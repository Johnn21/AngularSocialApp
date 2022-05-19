import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-forgot-password-confirm-code',
  templateUrl: './forgot-password-confirm-code.component.html',
  styleUrls: ['./forgot-password-confirm-code.component.css']
})
export class ForgotPasswordConfirmCodeComponent implements OnInit {
  codeForm: FormGroup;
  @Input() userEmail: string;

  constructor(private fb: FormBuilder, private accountService: AccountService, private stepper: CdkStepper) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.codeForm = this.fb.group({
      code: ['', Validators.required]
    })
  }

  submitCode() {
    this.accountService.checkResetPasswordCode(this.codeForm.get("code").value, this.userEmail).subscribe(() => {
      this.stepper.next();
    }, error => {
      this.codeForm.reset();
    })
  }

}
