import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserCredentials } from 'src/app/shared/contracts/userCredentials';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  invalidCredentials: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {

  }

  loginForm: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(6)]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)])
  });;

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        this.router.navigateByUrl("/");
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === HttpStatusCode.BadRequest) {
          this.loginForm.reset();
          this.invalidCredentials = true;
        }
      }
    });
  }
}
