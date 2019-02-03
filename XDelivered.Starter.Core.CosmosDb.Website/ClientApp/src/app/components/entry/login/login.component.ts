import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import swal from 'sweetalert2';
import { Router } from '@angular/router';
import { BaseComponent } from '../../baseComponent';
import { AuthService } from '../../../services/authService';
import { AdminAuthGuardService } from '../../../services/auth/adminGuard';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent extends BaseComponent implements OnInit {

  public isLoading: boolean;

  loginForm: FormGroup;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router) { super(); }

  ngOnInit() {
      this.loginForm = this.formBuilder.group({
          email: ['', [Validators.required, Validators.email]],
          password: ['', [Validators.required]]
      });
  }

  public login() {
    this.submitted = true;

    if (this.loginForm.invalid) {
        return;
    }

    this.isLoading = true;

    this.authService.login(this.loginForm.value.email, this.loginForm.value.password)
    .then(res => {
      if (res.isSuccess) {
        swal({
          type: 'success',
          title: 'success!'
        });
        this.isLoading = false;

        const currentRole = this.authService.getUser().role;
        if (currentRole === 'Owner') {
          this.router.navigate(['/main/my/restaurants']);
        } else {
          this.router.navigate(['/main/all']);
        }
      }
    }).catch(error => {
      this.isLoading = false;
      super.showError('Your login credentials failed');
    });

  }

  get f() { return this.loginForm.controls; }
}
