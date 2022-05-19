import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Friend } from 'src/app/models/friend';
import { Pagination } from 'src/app/models/pagination';

@Component({
  selector: 'app-profile-friends',
  templateUrl: './profile-friends.component.html',
  styleUrls: ['./profile-friends.component.css']
})
export class ProfileFriendsComponent implements OnInit {
  @Input() profileFriendsList: Friend[];
  @Input() pagination: Pagination;
  @Output() currentPage = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  pageChanged(event: any) {
    this.currentPage.emit(event.page)
  }

}
