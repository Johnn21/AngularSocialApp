import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css']
})
export class UpdateProfileComponent implements OnInit {
  currentUserName: string;
  currentUser: Member;
  updateProfileForm: FormGroup;
  user: User;

  constructor(private memberService: MemberService, private accountService: AccountService,
     private fb: FormBuilder, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getCurrentUserName();
    this.getUserByUsername();
  }

  private getCurrentUserName() {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.currentUserName = user.userName;
    });
  }

  private getUserByUsername() {
    this.memberService.getMemberByUsername(this.currentUserName).subscribe(user => {
      this.currentUser = user;
      console.log(this.currentUser);
      this.initializeForm();
    })
  }

  private initializeForm() {
    this.updateProfileForm = this.fb.group({
      gender: [this.currentUser.gender],
      dateOfBirth: [new Date(this.currentUser.dateOfBirth), Validators.required],
      firstName: [this.currentUser.firstName, Validators.required],
      lastName: [this.currentUser.lastName, Validators.required],
      phoneNumber: [this.currentUser.phoneNumber],
      displayName: [this.currentUser.displayName, Validators.required],
      description: [this.currentUser.description, Validators.required],
    })
  }

  updateProfile() {
    this.memberService.updateUser(this.updateProfileForm.value).subscribe( () =>{
      this.updateCurrentUserDisplayName();
      this.toastr.success('Your profile was updated!', 'Update profile');
    })
  }

  private updateCurrentUserDisplayName() {
    this.user.displayName = this.updateProfileForm.get('displayName').value;
    this.accountService.setCurrentUser(this.user);
  }

}
