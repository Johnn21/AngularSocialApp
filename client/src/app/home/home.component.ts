import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Friend } from '../models/friend';
import { Post } from '../models/post';
import { FriendService } from '../_services/friend.service';
import { PostService } from '../_services/post.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  posts: Post[] = [];
  friends: Friend[] = [];
  
  constructor(private router: Router, private toastr: ToastrService,
     private postService: PostService, private friendService: FriendService) { 
    if (this.router.getCurrentNavigation().extras.state && this.router.getCurrentNavigation().extras.state.post) {
      this.toastr.success("Post added successfully");
    }
  }

  ngOnInit(): void {
    this.getFriendsPosts();
    this.getFriendsList();
  }

  getFriendsPosts() {
    this.postService.getFriendsPost(0).subscribe((result) => {
      this.posts = result;
    });
  }

  getFriendsList() {
    this.friendService.getFriendsList().subscribe((result) => {
      this.friends = result;
    })
  }

}
