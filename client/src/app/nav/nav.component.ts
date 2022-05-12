import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  currentUser: User;
  searchedUsername: string;
  @ViewChild('searchMemberForm') searchMemberForm: NgForm;

  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.currentUser = user);
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  searchMember(){
    this.router.navigateByUrl('/profile/' + this.searchedUsername);
    this.searchMemberForm.reset();
  }

}
