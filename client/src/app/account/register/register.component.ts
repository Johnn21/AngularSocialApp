import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  validationErrors: string[] = [];
  public genderValues = ['male', 'female'];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      userName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      email: ['', Validators.required],
      displayName: ['', Validators.required],
      description: ['', Validators.required],
    })

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(resp => {
      this.router.navigateByUrl('/home');
    }, error => {
      this.validationErrors = error;
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value 
        ? null 
        : {isMatching: true}
    }
  }

}
