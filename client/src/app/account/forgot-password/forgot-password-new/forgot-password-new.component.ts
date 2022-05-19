import { Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-forgot-password-new',
  templateUrl: './forgot-password-new.component.html',
  styleUrls: ['./forgot-password-new.component.css']
})
export class ForgotPasswordNewComponent implements OnInit {
  newPasswordForm: FormGroup;
  @Input() userEmail: string;
  validationErrors: string[] = [];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.newPasswordForm = this.fb.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    })

    this.newPasswordForm.controls.password.valueChanges.subscribe(() => {
      this.newPasswordForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value 
        ? null 
        : {isMatching: true}
    }
  }

  submitPassword() {
    this.accountService.resetPasswordNew(this.userEmail, this.newPasswordForm.get('password').value).subscribe(() => {
      this.router.navigate(['login'], { state: { passwordChanged: 'password-changed' } });
    }, error => {
      this.validationErrors = error;
    })
  }

}
