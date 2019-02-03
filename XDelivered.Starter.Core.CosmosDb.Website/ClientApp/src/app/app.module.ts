import { AuthGuardService } from './services/auth/authGuard';

import { AdminUsersComponent } from './components/main/admin/users/users.component';
import { AuthService } from './services/authService';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { Location } from '@angular/common';

import { AppComponent } from './components/app.component';
import { Client } from '../clientapi/client';

import { LaddaModule } from 'angular2-ladda';
import { MainComponent } from './components/main/main.component';
import { EntryComponent } from './components/entry/entry.component';
import { LoginComponent } from './components/entry/login/login.component';
import { RegisterComponent } from './components/entry/register/register.component';

import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditUserComponent } from './components/main/admin/users/edituser.component';
import { OwnerAuthGuardService } from './services/auth/ownerGuard';
import { AdminAuthGuardService } from './services/auth/adminGuard';

export function getAuthService(http: HttpClient): Client {
  return new Client('http://localhost:3001');
}

@NgModule({
  declarations: [
    AppComponent,
    EntryComponent,
    LoginComponent,
    RegisterComponent,
    MainComponent,
    AdminUsersComponent,
    EditUserComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    LaddaModule,
    ReactiveFormsModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: EntryComponent },
      {
        path: 'main',
        component: MainComponent,
        canActivate: [AuthGuardService],
        children: [
          {
            path: '',
            redirectTo: 'all',
            pathMatch: 'full'
          },
          {
            path: 'admin',
            canActivate: [AuthGuardService, OwnerAuthGuardService, AdminAuthGuardService],
            children: [
              {
                path: 'users',
                component: AdminUsersComponent
              },
              {
                path: 'users/edit/:id',
                component: EditUserComponent
              },
            ]
          }
        ]
      }
    ])
  ],
  providers: [
    {
      provide: Client,
      useFactory: getAuthService,
      deps: [HttpClient]
    },
    AuthService,
    AuthGuardService,
    OwnerAuthGuardService,
    AdminAuthGuardService,
    Location
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
