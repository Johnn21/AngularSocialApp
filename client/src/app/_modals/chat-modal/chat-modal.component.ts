import { Component, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { take } from 'rxjs';
import { Friend } from 'src/app/models/friend';
import { Message } from 'src/app/models/message';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-chat-modal',
  templateUrl: './chat-modal.component.html',
  styleUrls: ['./chat-modal.component.css']
})
export class ChatModalComponent implements OnInit {
  content: string;
  friend: Friend;
  public messages: Message[] = [];
  public currentUserName: string;
  @ViewChild('sendMessageForm') sendMessageForm: NgForm;
  hideEvent: EventEmitter<any> = new EventEmitter();
  user: User;

  constructor(public bsModalRef: BsModalRef, public messageService: MessageService,
     private accountService: AccountService) {}

  ngOnInit(): void {
    // this.getMessagesBetweenUsers();
    this.getCurrentUser();
  }

  sendMessage() {
    this.messageService.sendMessage(this.content, this.friend.userName).subscribe((result) => {
      if (result) {
        this.messages = [...this.messages, result];
        this.sendMessageForm.reset();
      }
    })
  }

  sendMessageSignalR() {
    this.messageService.sendMessageSignalR(this.content, this.friend.userName).then(() => {
      this.sendMessageForm.reset();
    })
  }

  getMessagesBetweenUsers() {
    this.messageService.getMessagesBetweenusers(this.friend.userName).subscribe((result) => {
      if (result.length > 0) {
        this.messages = result;
        this.getCurrentUser();
      }
    })
  }

  ngOnDestroy(){
    this.hideEvent.next(null);
  }

  private getCurrentUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.currentUserName = user.userName;
      this.messageService.createHubConnection(user, this.friend.userName);
    });
  }

}