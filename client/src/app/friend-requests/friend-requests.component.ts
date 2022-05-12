import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { FriendRequestReceive } from '../models/friend-request-receive';
import { User } from '../models/user';
import { AccountService } from '../_services/account.service';
import { FriendService } from '../_services/friend.service';

@Component({
  selector: 'app-friend-requests',
  templateUrl: './friend-requests.component.html',
  styleUrls: ['./friend-requests.component.css']
})
export class FriendRequestsComponent implements OnInit {
  username: string;
  friendRequestsReceived:FriendRequestReceive [];
  public hasFriendshipRequests: boolean = false;
  user: User;

  constructor(private route: ActivatedRoute, private friendService: FriendService,
     private router: Router, private toastr: ToastrService, private accountService: AccountService) { 
    this.username = this.route.snapshot.paramMap.get('username');
  }

  ngOnInit(): void {
    this.getFriendshipRequests();
  }

  getFriendshipRequests() {
    this.friendService.getFriendshipRequests(this.username).subscribe((result) => {
      if (result.length > 0) {
        this.friendRequestsReceived = result;
        this.hasFriendshipRequests = true;
      }
    })
  }

  navigateToUserProfile(row: FriendRequestReceive) {
    this.router.navigateByUrl('/profile/' + row.senderUsername);
  }

  acceptFriendshipRequest(row: FriendRequestReceive) {
    this.friendService.addFriendship(row.senderUsername).subscribe((response) => {
      if (response) {
        this.friendRequestsReceived = this.friendRequestsReceived.filter(x => x.senderUsername !== row.senderUsername);
        this.getCurrentUser();
        if(this.user.friendRequestsCount > 0) {
          this.user.friendRequestsCount--;
          if(this.user.friendRequestsCount == 0) {
            this.hasFriendshipRequests = false;
          }
        }
        this.accountService.setCurrentUser(this.user);
        this.toastr.success("Friendship created");
      }
    })
  }

  rejectFriendshipRequest(row: FriendRequestReceive) {
    this.friendService.rejectFriendship(row.senderUsername).subscribe((response) => {
      if (response) {
        this.friendRequestsReceived = this.friendRequestsReceived.filter(x => x.senderUsername !== row.senderUsername);
        this.getCurrentUser();
        if(this.user.friendRequestsCount > 0) {
          this.user.friendRequestsCount--;
          if(this.user.friendRequestsCount == 0) {
            this.hasFriendshipRequests = false;
          }
        }
        this.accountService.setCurrentUser(this.user);
        this.toastr.success("Friendship rejected");
      }
    })
  }

  private getCurrentUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
  }

}
