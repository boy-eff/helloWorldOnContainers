import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsersService } from 'src/app/services/users.service';
import {
  EnglishLevelOption,
  englishLevels,
} from 'src/app/shared/contracts/englishLevelOption';
import { RegisterUser } from 'src/app/shared/contracts/registerUser';
import { MatchValidator } from 'src/app/shared/validators/match.validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  passwordControlName = 'password';
  confirmPasswordControlName = 'confirmPassword';
  selectedLevel: number;
  levels: EnglishLevelOption[] = englishLevels;
  conflictResponse: boolean = false;

  constructor(private usersService: UsersService, private router: Router) {}

  registerForm: FormGroup = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    confirmPassword: new FormControl('', [
      MatchValidator(this.passwordControlName),
    ]),
    englishLevel: new FormControl('', [Validators.required]),
  });

  onSubmit() {
    let value = this.registerForm.value;
    let user: RegisterUser = {
      username: value.username,
      password: value.password,
      englishLevel: value.englishLevel,
    };
    this.usersService.registerUser(user).subscribe({
      next: () => {
        this.router.navigateByUrl('/login');
      },
      error: (err: HttpErrorResponse) => {
        if (err.status == HttpStatusCode.Conflict) {
          this.conflictResponse = true;
        }
      },
    });
  }
}
