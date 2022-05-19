import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private accountService: AccountService,
     private router: Router, private toastr: ToastrService) {
    if (this.router.getCurrentNavigation().extras.state && this.router.getCurrentNavigation().extras.state.post) {
      this.toastr.success("Account created successfully", "Please confirm your email address");
    }
    if (this.router.getCurrentNavigation().extras.state && this.router.getCurrentNavigation().extras.state.passwordChanged) {
      this.toastr.success("Password changed successfully", "You can login with your new password");
    }
   }

  ngOnInit(): void {
    this.initializeForm();
  }

  login() {
    this.accountService.login(this.loginForm.value).subscribe(() => {
      this.router.navigateByUrl('/home');
    })
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
    })
  }

}
