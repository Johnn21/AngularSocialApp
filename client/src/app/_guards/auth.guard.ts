import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private toastr: ToastrService, private router: Router) {}

  canActivate(): boolean {

    let currentUser = localStorage.getItem('user');
    if (currentUser){
      if (currentUser) return true;
    } else {
      this.toastr.error("Please login!");
      this.router.navigateByUrl('/');
    }
  
  }
  
}
