import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsersService } from 'src/app/services/users.service';
import { EnglishLevel } from 'src/app/shared/enums/EnglishLevel';
import { Alphabets } from 'src/app/shared/enums/Test';
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
  levels: { value: number; label: string }[];
  conflictResponse: boolean = false;

  constructor(private usersService: UsersService, private router: Router) {
    this.levels = Object.keys(EnglishLevel)
      .filter((k) => parseInt(k) >= 0)
      .map((k) => ({ value: Number(k), label: EnglishLevel[Number(k)] }));
  }

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
    this.usersService.registerUser(this.registerForm.value).subscribe({
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
