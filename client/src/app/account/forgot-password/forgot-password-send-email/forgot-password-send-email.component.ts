import { CdkStepper } from '@angular/cdk/stepper';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-forgot-password-send-email',
  templateUrl: './forgot-password-send-email.component.html',
  styleUrls: ['./forgot-password-send-email.component.css']
})
export class ForgotPasswordSendEmailComponent implements OnInit {
  emailForm: FormGroup;
  @Output() userEmail = new EventEmitter();

  constructor(private fb: FormBuilder, private accountService: AccountService,
     private stepper: CdkStepper, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.emailForm = this.fb.group({
      email: ['', Validators.required]
    })
  }

  sendResetPasswordCodeToEmail() {
    this.accountService.sendResetPasswordCodeToEmail(this.emailForm.get('email').value).subscribe((result) => {
      this.userEmail.emit(this.emailForm.get('email').value);
      this.stepper.next();
      this.toastr.success("An email with the reset code has been send to you");
    }, error => {
      this.emailForm.reset();
    })
  }

}
