import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from '../models/member';
import { User } from '../models/user';
import { FriendRequestState } from '../_constants/friend_request_state';
import { AccountService } from '../_services/account.service';
import { FriendService } from '../_services/friend.service';
import { MemberService } from '../_services/member.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  username: string;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  user: User;

  friendRequestStates = {
    None: FriendRequestState.None,
    Requested: FriendRequestState.Requested,
    NonRequested: FriendRequestState.NonRequested,
    Received: FriendRequestState.Received,
    NonReceived: FriendRequestState.NonReceived,
    Friends: FriendRequestState.Friends
  }


  constructor(private memberService: MemberService, private route: ActivatedRoute,
       private router: Router, private toastr: ToastrService, private friendService: FriendService,
       private accountService: AccountService) {
    this.username = this.route.snapshot.paramMap.get('username');
   }

  ngOnInit(): void {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.getMemberByUsername();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for(const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }

  getMemberByUsername() {
    this.memberService.getMemberByUsername(this.username).subscribe(member => {
      this.member = member;
      console.log(this.member);
      if (this.member) {
        this.galleryImages = this.getImages();
      }
    })
  }

  sendFriendshipRequest() {
    this.friendService.sendFriendshipRequest(this.username).subscribe(() => {
      this.toastr.success("Friendship request sended");
      this.member.friendshipRequestStatus = FriendRequestState.Requested;
    })
  }

  acceptFriendshipRequest() {
    this.friendService.addFriendship(this.member.userName).subscribe((response) => {
      if (response) {
        this.getCurrentUser();
        if(this.user.friendRequestsCount > 0) {
          this.user.friendRequestsCount--;
        }
        this.accountService.setCurrentUser(this.user);
        this.toastr.success("Friendship created");
      }
    })
    this.member.friendshipRequestStatus = FriendRequestState.Friends;
  }

  private getCurrentUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
  }

}
