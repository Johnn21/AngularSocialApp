import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Like } from 'src/app/models/like';
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

  constructor(private postService: PostService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  getMorePosts() {
    this.skipPosts++;
    this.postService.getFriendsPost(this.skipPosts).subscribe((result) => {
      if (result.length === 0){
        this.noMorePosts = true;
      } else {
        this.posts = [...this.posts, ...result];
      }
    })
  }
 
  addLikeToPost(post: Post) {
    this.postService.addLikeToPost(post.id).subscribe((result) => {
      if (result) { 
        
        let removeLike = false;
        if (post.likes.length > 0 && post.likes?.[0].liked) {
          if(post.likesCount > 0) post.likesCount--;
          
          post.likes = [];
          removeLike = true;
        }

        if (post.likes.length > 0 && !post.likes?.[0].liked) {
          if(post.dislikesCount > 0) post.dislikesCount--;
          
          post.likesCount++;

          post.likes[0].liked = true;

          this.toastr.success("You liked this post")
        }
        
        if (post.likes.length === 0 && !removeLike) {
          let like = new Like();
          like.liked = true;
          post.likes[0] = like;
          post.likesCount++;

          this.toastr.success("You liked this post")
        }

        let itemIndex = this.posts.findIndex(item => item.id == post.id);
        this.posts[itemIndex] = post;
      }
    })
  }

  addDislikeToPost(post: Post) {
    this.postService.addDislikeToPost(post.id).subscribe((result) => {
      if (result) {

        let removeDislike = false;
        if (post.likes.length > 0 && !post.likes?.[0].liked) {
          if(post.dislikesCount > 0) post.dislikesCount--;
          
          post.likes = [];
          removeDislike = true;
        }

        if (post.likes.length > 0 && post.likes?.[0].liked) {
          if (post.likesCount > 0) post.likesCount--;

          post.dislikesCount++;

          post.likes[0].liked = false;

          this.toastr.success("You disliked this post")
        }
        
        if (post.likes.length === 0 && !removeDislike) {
          let like = new Like();
          like.liked = false;
          post.likes[0] = like;
          post.dislikesCount++;

          this.toastr.success("You disliked this post")
        }
        
        let itemIndex = this.posts.findIndex(item => item.id == post.id);
        this.posts[itemIndex] = post;
      }
    })
  }

}
