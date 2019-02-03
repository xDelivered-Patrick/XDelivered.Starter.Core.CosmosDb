import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../authService';
@Injectable()
export class OwnerAuthGuardService implements CanActivate {
  constructor(public auth: AuthService, public router: Router) {}

  canActivate(): boolean {
    if (this.auth.getUser().role === 'User') {
      this.router.navigate(['']);
      return false;
    }
    return true;
  }
}
