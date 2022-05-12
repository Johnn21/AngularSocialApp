import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Friend } from 'src/app/models/friend';
import { ChatModalComponent } from 'src/app/_modals/chat-modal/chat-modal.component';
import { FriendService } from 'src/app/_services/friend.service';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent implements OnInit {
  friends: Friend[] = [];
  bsModalRef: BsModalRef;

  constructor(private friendService: FriendService, private router: Router,
     private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getFriendsList();
  }

  getFriendsList() {
    this.friendService.getFriendsList().subscribe((result) => {
      this.friends = result;
    })
  }
  
  navigateToUserProfile(friend: Friend) {
    this.router.navigateByUrl('/profile/' + friend.userName);
  }

  openChatModal(friend: Friend) {
    const config = {
      class: 'modal-lg',
      initialState: {
        friend
      }
    }
    this.bsModalRef = this.modalService.show(ChatModalComponent, config);
  }

}
