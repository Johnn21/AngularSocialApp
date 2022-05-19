import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { map, take } from 'rxjs';
import { Friend } from '../models/friend';
import { Member } from '../models/member';
import { Pagination } from '../models/pagination';
import { User } from '../models/user';
import { FriendParams } from '../params/friendParams';
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
  profileFriendsList: Friend[] = [];
  pagination: Pagination;
  friendParams: FriendParams;

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

    this.friendParams = new FriendParams();
  }

  onTabActivated(event) {
    if(event.heading === 'Friends') {
      this.getProfileFriendsList();
    }
  }

  getProfileFriendsList() {
    this.friendService.getPaginatedProfileFriendsList(this.username, this.friendParams).subscribe((result) => {
      this.profileFriendsList = result.result;
      this.pagination = result.pagination;
    })
  }

  pageChangedEvent(event: number) {
    this.friendParams.pageNumber = event;
    this.getProfileFriendsList();
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
