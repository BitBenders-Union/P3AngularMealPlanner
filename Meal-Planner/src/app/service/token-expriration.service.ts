import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginService } from '../service/login.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenExpirationService {

  constructor(
    private jwtHelper: JwtHelperService,
    private auth: LoginService,
    private router: Router
  ) { }

  checkTokenExpiration() {
    const token = this.auth.getToken();
    
    if (token) {
      const tokenExpired = this.jwtHelper.isTokenExpired(token);

      if (tokenExpired) {
        // Token has expired, sign out the user
        this.auth.signOut();
        this.router.navigate(['Login']);
      }
    }
  }
}
