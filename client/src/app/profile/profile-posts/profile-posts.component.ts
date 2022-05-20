import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Pagination } from 'src/app/models/pagination';
import { Post } from 'src/app/models/post';

@Component({
  selector: 'app-profile-posts',
  templateUrl: './profile-posts.component.html',
  styleUrls: ['./profile-posts.component.css']
})
export class ProfilePostsComponent implements OnInit {
  @Input() profilePostsList: Post[];
  @Input() postPagination: Pagination;
  @Output() currentPagePosts = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  pageChanged(event: any) {
    this.currentPagePosts.emit(event.page)
  }

}
