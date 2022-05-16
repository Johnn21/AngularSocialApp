import { Component, Input, OnInit } from '@angular/core';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-posts-wall',
  templateUrl: './posts-wall.component.html',
  styleUrls: ['./posts-wall.component.css']
})
export class PostsWallComponent implements OnInit {
  @Input() posts: Post[];
  skipPosts: number = 0;
  noMorePosts: boolean = false;

  constructor(private postService: PostService) { }

  ngOnInit(): void {
  }

  getMorePosts() {
    this.skipPosts++;
    this.postService.getFriendsPost(this.skipPosts).subscribe((result) => {
      // this.results = [ ...this.results, ...data.results];
      if (result.length === 0){
        this.noMorePosts = true;
      } else {
        this.posts = [...this.posts, ...result];
      }
    })
  }

}
