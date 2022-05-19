import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService, private router: Router) { }

  register(model: any) {
    return this.http.post(this.apiUrl + 'account/register', model).pipe(
      map((result: boolean) => {
        if(result){
          return true;
        }
      })
    )
  }

  login(model: any) {
    return this.http.post(this.apiUrl + 'account/login', model).pipe(
      map((user: User) => {
        if(user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    )
  }

  activateAccount(accountConfirmationToken: string, username: string) {
    return this.http.post(this.apiUrl + 'account/activate-account', {accountConfirmationToken, username});
  }

  sendResetPasswordCodeToEmail(email: any) {
    return this.http.post(this.apiUrl + 'account/reset-password-email-confirmation/' + email, {});
  }

  checkResetPasswordCode(code: string, email: string) {
    return this.http.post(this.apiUrl + 'account/reset-password-check-code/' + code + '/' + email, {});
  }

  resetPasswordNew(email: string, password: string) {
    return this.http.post(this.apiUrl + 'account/reset-password-new', {email, password});
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
    this.presence.stopHubConnection();
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
