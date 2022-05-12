import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          console.log(error)
          switch(error.status) {
            case 400:
              if (Array.isArray(error.error)) {
                const modalStateErrors = [];
                for (const key in error.error) {
                  if (error.error[key]) {
                    modalStateErrors.push(error.error[key]);
                  }
                }
                throw modalStateErrors.flat();
              }
            else if (typeof(error.error) === 'object') {
              this.toastr.error(error.statusText === "OK" ? "Bad Request" : error.statusText, error.status);
            } else {
              this.toastr.error(error.error, error.status);
            }
            break;
            case 401: 
              if(error.error === "Invalid credentials"){
                this.toastr.error(error.statusText, "Invalid credentials");
              }else{
                this.toastr.error(error.statusText === "OK" ? "Unauthorized" : error.statusText, error.status);
              }
            break;
            case 404:
              this.toastr.error("Not found");
            break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
            break;
          }
        }
        return throwError(error);
      })
    )
  }
}
