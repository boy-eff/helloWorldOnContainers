import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserModel } from 'src/app/shared/contracts/userModel';
import { EnglishLevel } from 'src/app/shared/enums/EnglishLevel';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss'],
})
export class AccountSettingsComponent implements OnInit {
  englishLevels = EnglishLevel;
  user: UserModel | null;

  constructor(private authService: AuthenticationService) {}
  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      console.log(user);
      this.user = user;
    });
  }

  form = new FormGroup({
    password: new FormControl(''),
    englishLevel: new FormControl(EnglishLevel.Elementary),
    image: new FormControl(null),
  });

  onSubmit() {
    console.log(this.form.value);
  }
}
