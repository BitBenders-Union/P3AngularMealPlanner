import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { LoginService } from '../login.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: LoginService, private router: Router) {}

  // send token with every request
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();

    if(myToken){
      request = request.clone({
        setHeaders: {Authorization:`Bearer ${myToken}`}
      })
    }
    return next.handle(request).pipe(
      catchError((error: any) => {
        if(error instanceof HttpErrorResponse){
          if(error.status === 401){
            this.auth.signOut();
            this.router.navigate(['login']);
          }
        }

        return throwError(() => new Error("Server Error"));
      })
    );
  }
}
