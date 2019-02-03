import { BaseComponent } from './../../baseComponent';
import {
  Client,
  RegisterRequestModel
} from './../../../../clientapi/client';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent extends BaseComponent implements OnInit {
  public roles = [{id: 0, name: 'Reviewer'}, {id: 1, name: 'Owner'}];
  public selectedRole = null;

  registerForm: FormGroup;
  submitted = false;
  isLoading = false;

  constructor(private formBuilder: FormBuilder, private client: Client) {
    super();
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group(
      {
        name: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
        password2: ['', [Validators.required]],
        roleControl: [this.roles[0]]
      },
      { validator: this.checkPasswords }
    );
  }

  public async register() {
    this.submitted = true;

    if (this.registerForm.invalid) {
      return;
    }

    this.isLoading = true;

    const registerResult = await super.attemptConnection(this.client.apiAccountRegisterPost(
      new RegisterRequestModel({
        name: this.registerForm.value.name,
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        role: this.registerForm.value.roleControl.id
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

  checkPasswords(group: FormGroup) {
    // here we have the 'passwords' group
    const pass = group.controls.password.value;
    const confirmPass = group.controls.password2.value;

    return pass === confirmPass ? null : { notSame: true };
  }

  get f() {
    return this.registerForm.controls;
  }
}
