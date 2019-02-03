import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../authService';
@Injectable()
export class AdminAuthGuardService implements CanActivate {
  constructor(public auth: AuthService, public router: Router) {}

  canActivate(): boolean {
    if (this.auth.getUser().role !== 'Admin') {
      this.router.navigate(['']);
      return false;
    }
    return true;
  }
}
