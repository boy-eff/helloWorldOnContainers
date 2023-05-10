import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, of, tap } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styleUrls: ['./change-password-modal.component.scss'],
})
export class ChangePasswordModalComponent {
  changePasswordForm: FormGroup = new FormGroup({
    oldPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    newPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  invalidPassword: boolean = false;

  constructor(
    public activeModal: NgbActiveModal,
    private accountService: AccountService
  ) {}

  onSubmit() {
    let oldPassword = this.changePasswordForm.controls['oldPassword'].value;
    let newPassword = this.changePasswordForm.controls['newPassword'].value;
    return this.accountService
      .changePassword(oldPassword, newPassword)
      .pipe(
        tap((response) => {
          this.activeModal.close();
        }),
        catchError((err: HttpErrorResponse) => {
          if (err.status == HttpStatusCode.BadRequest) {
            this.invalidPassword = true;
          }
          return of(`An error occured: ${err.message}`);
        })
      )
      .subscribe((x) => {});
  }
}
