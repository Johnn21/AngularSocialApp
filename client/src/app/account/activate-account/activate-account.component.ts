import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html',
  styleUrls: ['./activate-account.component.css']
})
export class ActivateAccountComponent implements OnInit {
  private accountConfirmationToken: string;
  private userName: string;
  accountConfirmed: boolean = false;

  constructor(private route: ActivatedRoute, private accountService: AccountService, private router: Router) { 
    this.accountConfirmationToken = this.route.snapshot.paramMap.get('confirmationToken');
    this.userName = this.route.snapshot.paramMap.get('username');

    this.activateAccount();
  }

  ngOnInit(): void {
  }

  activateAccount() {
    this.accountService.activateAccount(this.accountConfirmationToken, this.userName).subscribe((result) => {
      if (result) {
        this.accountConfirmed = true;
      }
    });
  }

  goToLogin() {
    this.router.navigateByUrl('login');
  }

}
