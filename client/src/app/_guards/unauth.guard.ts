import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UnauthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    let currentUser = localStorage.getItem('user');
    if (!currentUser) {
      return true;
    }

    this.router.navigateByUrl('/home');
    return false;
  }
  
}
