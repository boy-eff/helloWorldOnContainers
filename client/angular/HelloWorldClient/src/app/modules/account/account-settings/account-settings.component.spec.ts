import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject, of } from 'rxjs';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { AccountService } from 'src/app/services/account.service';
import { ChangePasswordModalComponent } from './change-password-modal/change-password-modal.component';
import { AccountSettingsComponent } from './account-settings.component';
import { User } from 'src/app/shared/contracts/user';

describe('AccountSettingsComponent', () => {
  let component: AccountSettingsComponent;
  let fixture: ComponentFixture<AccountSettingsComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
  let accountServiceSpy: jasmine.SpyObj<AccountService>;
  let modalServiceSpy: jasmine.SpyObj<NgbModal>;
  const user: User = {
    id: 1,
    userName: 'testuser',
    englishLevel: EnglishLevel.Intermediate,
    imageUrl: 'testurl',
  };

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthenticationService', [
      'refreshUser',
    ]);
    accountServiceSpy = jasmine.createSpyObj('AccountService', ['changeImage']);
    modalServiceSpy = jasmine.createSpyObj('NgbModal', ['open']);

    await TestBed.configureTestingModule({
      imports: [NgbModule],
      declarations: [AccountSettingsComponent],
      providers: [
        { provide: AuthenticationService, useValue: authServiceSpy },
        { provide: AccountService, useValue: accountServiceSpy },
        { provide: NgbModal, useValue: modalServiceSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSettingsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set user and englishLevel when currentUser$ emits', () => {
    authServiceSpy.currentUser$ = of(user);

    fixture.detectChanges();

    expect(component.user).toEqual(user);
    expect(component.englishLevel).toEqual(user.englishLevel);
  });

  it('should call accountService.changeImage and authService.refreshUser when onFileChange is called with a file', () => {
    const file = new File(['test'], 'test.png', { type: 'image/png' });
    const refreshUser$ = of(user);
    authServiceSpy.refreshUser.and.returnValue(refreshUser$);
    accountServiceSpy.changeImage.and.returnValue(of({}));

    component.onFileChange({ target: { files: [file] } });

    expect(accountServiceSpy.changeImage).toHaveBeenCalledWith(file);
    expect(authServiceSpy.refreshUser).toHaveBeenCalled();
  });
});
