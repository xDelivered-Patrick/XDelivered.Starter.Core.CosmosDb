import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/authService';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (!AuthService.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
