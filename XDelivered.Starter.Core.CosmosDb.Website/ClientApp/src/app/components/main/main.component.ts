import { UserInfoResponseModel } from './../../../clientapi/client';
import { AuthService } from './../../services/authService';
import { BaseComponent } from './../baseComponent';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
})
export class MainComponent extends BaseComponent implements OnInit {

  user: UserInfoResponseModel;

  constructor(private authService: AuthService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    if (!AuthService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
    this.user = this.authService.getUser();
  }

  public logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
