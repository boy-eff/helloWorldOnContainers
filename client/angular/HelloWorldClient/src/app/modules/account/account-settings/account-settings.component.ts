import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { User } from 'src/app/shared/contracts/user';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';
import { ChangePasswordModalComponent } from './change-password-modal/change-password-modal.component';
import { AccountService } from 'src/app/services/account.service';
import { switchMap } from 'rxjs';
import {
  EnglishLevelOption,
  englishLevels,
} from 'src/app/shared/contracts/englishLevelOption';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss'],
})
export class AccountSettingsComponent implements OnInit {
  englishLevels: EnglishLevelOption[] = englishLevels;
  user: User | null;

  englishLevel: EnglishLevel;

  constructor(
    private authService: AuthenticationService,
    private modalService: NgbModal,
    private accountService: AccountService
  ) {}
  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.user = user;
      if (user) {
        console.log(user.englishLevel);
        this.englishLevel = user.englishLevel;
      }
    });
  }

  openChangePasswordModal(): void {
    const modalRef = this.modalService.open(ChangePasswordModalComponent);
    modalRef.result.then(
      (result) => {},
      (reason) => {}
    );
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.accountService
        .changeImage(file)
        .pipe(
          switchMap(() => {
            return this.authService.refreshUser();
          })
        )
        .subscribe((x) => {});
    }
  }
}
