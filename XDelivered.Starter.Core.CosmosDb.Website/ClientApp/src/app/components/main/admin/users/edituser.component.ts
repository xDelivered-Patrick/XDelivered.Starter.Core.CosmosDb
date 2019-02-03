import { ActivatedRoute, Router } from '@angular/router';

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import swal from 'sweetalert2';
import { BaseComponent } from '../../../baseComponent';
import { Client, UserModel } from '../../../../../clientapi/client';

@Component({
  selector: 'app-edituser',
  templateUrl: './edituser.component.html'
})
export class EditUserComponent extends BaseComponent implements OnInit {
  public roles = [{id: 0, name: 'User'}, {id: 1, name: 'Owner'}, {id: 2, name: 'Admin'}];
  public selectedRole = null;

  form: FormGroup;
  submitted = false;
  isLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private client: Client,
    private router: Router,
    private route: ActivatedRoute) {
    super();
  }

  ngOnInit() {
    this.loadData();
  }

  private async loadData() {
    const id = this.route.snapshot.paramMap.get('id');

    const user = await super.assertResult<UserModel>(await super.attemptConnection(this.client.apiAdminUserByIdGet(id)));

    this.form = this.formBuilder.group(
      {
        name: [user.name, [Validators.required]],
        email: [user.email, [Validators.required, Validators.email]],
        password: ['', []],
        roleControl: [this.roles[user.role === 'User' ? 0 : (user.role === 'Owner' ? 1 : (user.role === 'Admin' ? 2 : -1))]]
      }
    );
  }

  public async delete() {
    const id = this.route.snapshot.paramMap.get('id');
    this.attemptConnection(this.client.apiAdminUsersByIdDelete(id).then(() => {
      this.router.navigate(['/main/admin/users']);
    }));
  }

  public async update() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.isLoading = true;

    const id = this.route.snapshot.paramMap.get('id');

    const registerResult = await super.attemptConnection(this.client.apiAdminUsersByIdPost(
      id,
      new UserModel({
        id: id,
        name: this.form.value.name,
        email: this.form.value.email,
        password: this.form.value.password,
        role: this.form.value.roleControl.id
      })
    )).catch(reject => {
      this.isLoading = false;
    });

    super.assertResult(registerResult).then(() => {
      swal({
        type: 'success',
        title: 'success!'
      });
    });

    this.isLoading = false;
  }

  get f() {
    return this.form.controls;
  }
}
