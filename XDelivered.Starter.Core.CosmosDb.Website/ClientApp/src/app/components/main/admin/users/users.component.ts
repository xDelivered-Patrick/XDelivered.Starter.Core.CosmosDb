import { AuthService } from './../../../../services/authService';
import { UserModel } from './../../../../../clientapi/client';
import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../baseComponent';
import { Client } from '../../../../../clientapi/client';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-users',
  templateUrl: './users.component.html'
})
export class AdminUsersComponent extends BaseComponent implements OnInit {
  private _client: Client;
  users: UserModel[];

  constructor(
    client: Client,
    private authService: AuthService,
    private router: Router) {
    super();
    this._client = client;
  }

  ngOnInit(): void {
    this.loadData();
  }

  public load(data: UserModel) {
    this.router.navigate(['main/admin/users/edit', data.id]);
  }

  private async loadData(filterMin?, filterMax?) {
    this.users = await super.assertResult<UserModel[]>(
      await this._client.apiAdminUsersGet()
    );
  }
}
