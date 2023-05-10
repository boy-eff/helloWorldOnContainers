import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { of, throwError } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';
import { ChangePasswordModalComponent } from './change-password-modal.component';

describe('ChangePasswordModalComponent', () => {
  let component: ChangePasswordModalComponent;
  let fixture: ComponentFixture<ChangePasswordModalComponent>;
  let AccountServiceSpy: jasmine.SpyObj<AccountService>;
  let ActiveModalSpy: jasmine.SpyObj<NgbActiveModal>;

  beforeEach(async () => {
    AccountServiceSpy = jasmine.createSpyObj('AccountService', [
      'changePassword',
    ]);
    ActiveModalSpy = jasmine.createSpyObj('NgbActiveModal', [
      'close',
      'dismiss',
    ]);

    await TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule],
      declarations: [ChangePasswordModalComponent],
      providers: [
        { provide: AccountService, useValue: AccountServiceSpy },
        { provide: NgbActiveModal, useValue: ActiveModalSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangePasswordModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should close the modal when the form is submitted successfully', () => {
    const oldPassword = 'password123';
    const newPassword = 'newpassword123';
    AccountServiceSpy.changePassword.and.returnValue(of(1));

    component.changePasswordForm.controls['oldPassword'].setValue(oldPassword);
    component.changePasswordForm.controls['newPassword'].setValue(newPassword);
    component.onSubmit();

    expect(AccountServiceSpy.changePassword).toHaveBeenCalledWith(
      oldPassword,
      newPassword
    );
    expect(component.activeModal.close).toHaveBeenCalled();
  });

  it('should set invalidPassword to true when the API returns a 400 Bad Request error', () => {
    const errorResponse = { status: 400, error: 'Bad Request' };
    AccountServiceSpy.changePassword.and.returnValue(
      throwError(() => errorResponse)
    );

    component.onSubmit();

    expect(component.invalidPassword).toBeTrue();
  });

  it('should not set invalidPassword when the API returns an error other than 400 Bad Request', () => {
    const errorResponse = { status: 500, error: 'Internal Server Error' };
    AccountServiceSpy.changePassword.and.returnValue(
      throwError(() => errorResponse)
    );

    component.onSubmit();

    expect(component.invalidPassword).toBeFalse();
  });
});
