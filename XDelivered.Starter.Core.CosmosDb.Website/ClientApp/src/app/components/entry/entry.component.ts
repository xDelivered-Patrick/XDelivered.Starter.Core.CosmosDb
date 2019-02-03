import { AuthService } from './../../services/authService';
import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../baseComponent';
import { Router } from '@angular/router';
declare var SEMICOLON;

@Component({
  selector: 'app-entry',
  templateUrl: './entry.component.html'
})
export class EntryComponent extends BaseComponent implements OnInit {
  constructor(authService: AuthService, router: Router) {
    super();
    if (AuthService.isLoggedIn()) {
      router.navigate(['/main']);
    }
  }

  ngOnInit(): void {
    // canvas UI - ensure init even on logout
    SEMICOLON.widget.tabs();
    SEMICOLON.widget.tabsJustify();
    SEMICOLON.widget.tabsResponsive();
    SEMICOLON.widget.tabsResponsiveResize();
  }
}
