import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { User } from 'src/app/shared/contracts/user';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss'],
})
export class AccountSettingsComponent implements OnInit {
  englishLevels = EnglishLevel;
  user: User | null;

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
