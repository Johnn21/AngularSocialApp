import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PostComment } from 'src/app/models/post-comment';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-post-comment',
  templateUrl: './post-comment.component.html',
  styleUrls: ['./post-comment.component.css']
})
export class PostCommentComponent implements OnInit {
  @Input() postId: number;
  postComments: PostComment[] = [];

  @ViewChild('addedPostComment') postComment;

  constructor(private postService: PostService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getPostComments();
  }

  submitPostComment() {
    this.postService.addPostComment(this.postId, this.postComment.nativeElement.value).subscribe((result) => {
      if (result) {
        this.toastr.success("Post comment added successfully");
        this.postComment.nativeElement.value = "";

        this.postComments.unshift(result);
      }
    })
  }

  getPostComments() {
    this.postService.getPostComments(this.postId).subscribe((result) => {
      this.postComments = result;
    })
  }

}
