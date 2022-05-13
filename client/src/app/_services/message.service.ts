import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../models/group';
import { Message } from '../models/message';
import { User } from '../models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection.start().catch(error => console.log(error));

      this.hubConnection.on('ReceiveMessageThread', messages => {
        this.messageThreadSource.next(messages);
      })

      this.hubConnection.on('NewMessage', message => {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          this.messageThreadSource.next([...messages, message]);
        })
      })

      this.hubConnection.on('UpdatedGroup', (group: Group) => {
        if (group.connections.some(x => x.username === otherUsername)) {
          this.messageThread$.pipe(take(1)).subscribe(messages => {
            messages.forEach(message => {
              if (!message.dateMessageRead) {
                message.dateMessageRead = new Date(Date.now());
              }
            })

            this.messageThreadSource.next([...messages]);
          })
        }
      })
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  async sendMessage(content: string, receiverUsername: string) {
    const data = {'content': content, 'receiverUsername': receiverUsername};
    return this.hubConnection.invoke('SendMessage', data)
      .catch(error => console.log(error));
  }

  async sendTypingInfo(content: string, receiverUsername: string) {
    const data = {'content': content, 'receiverUsername': receiverUsername};
    return this.hubConnection.invoke('SenderIsTyping', data)
      .catch(error => console.log(error));
  }
}
