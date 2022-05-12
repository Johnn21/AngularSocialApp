import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  register(model: any) {
    return this.http.post(this.apiUrl + 'account/register', model).pipe(
      map((user: User) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  login(model: any) {
    return this.http.post(this.apiUrl + 'account/login', model).pipe(
      map((user: User) => {
        console.log(user);
        if(user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
