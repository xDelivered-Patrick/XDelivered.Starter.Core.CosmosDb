import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../authService';
@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(public router: Router) {}

  canActivate(): boolean {
    if (!AuthService.isLoggedIn()) {
      this.router.navigate(['']);
      return false;
    }
    return true;
  }
}
