import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  sendMessage(content: string, receiverUsername: string) {
    const data = {'content': content, 'receiverUsername': receiverUsername};
    return this.http.post<any>(this.apiUrl + 'message/send-message', data); 
  }

  getMessagesBetweenusers(username: string) {
    return this.http.get<Message[]>(this.apiUrl + 'message/get-messages-between-users/' + username);
  }
}
