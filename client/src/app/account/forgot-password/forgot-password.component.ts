import { ChangeDetectorRef, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  public userEmail: string = "asd";

  constructor(private cdref: ChangeDetectorRef) { }

  ngOnInit(): void {
  }

  mailSendEvent(event:string) {
    this.userEmail = event;
  }

  ngAfterContentChecked() {

    this.cdref.detectChanges();

  }

}
