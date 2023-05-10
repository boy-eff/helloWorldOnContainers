import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { LoginComponent } from './login.component';
import { User } from 'src/app/shared/contracts/user';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
  let routerSpy: jasmine.SpyObj<Router>;
  const mockUser: User = {
    id: 1,
    userName: 'testUser',
    englishLevel: EnglishLevel.Advanced,
    imageUrl: 'https://example.com/profile.jpg',
  };

  beforeEach(() => {
    authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [LoginComponent],
      providers: [
        { provide: AuthenticationService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('onSubmit', () => {
    it('should call the login method of the authentication service with the form value', () => {
      const formValue = { username: 'testuser', password: 'testpassword' };
      authServiceSpy.login.and.returnValue(of(mockUser));

      component.loginForm.setValue(formValue);
      component.onSubmit();

      expect(authServiceSpy.login).toHaveBeenCalledWith(formValue);
    });

    it('should navigate to "/" if the login is successful', () => {
      authServiceSpy.login.and.returnValue(of(mockUser));

      component.onSubmit();

      expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/');
    });

    it('should reset the form and set invalidCredentials to true if the login fails', () => {
      authServiceSpy.login.and.returnValue(
        throwError(() => {
          return { status: 400 };
        })
      );

      component.onSubmit();

      expect(component.loginForm.value).toEqual({
        username: null,
        password: null,
      });
      expect(component.invalidCredentials).toBeTrue();
    });
  });

  describe('username form control', () => {
    let usernameControl: AbstractControl;

    beforeEach(() => {
      usernameControl = component.loginForm.controls['username'];
    });

    it('should be invalid when empty', () => {
      usernameControl.setValue('');

      expect(usernameControl.valid).toBeFalse();
      expect(usernameControl?.errors?.['required']).toBeTrue();
    });

    it('should be invalid when less than 6 characters', () => {
      usernameControl.setValue('12345');

      expect(usernameControl.valid).toBeFalse();
      expect(usernameControl?.errors?.['minlength']).toBeTruthy();
    });

    it('should be valid when 6 or more characters', () => {
      usernameControl.setValue('123456');

      expect(usernameControl.valid).toBeTrue();
      expect(usernameControl.errors).toBeNull();
    });
  });

  describe('password form control', () => {
    let passwordControl: AbstractControl;

    beforeEach(() => {
      passwordControl = component.loginForm.controls['password'];
    });

    it('should be invalid when empty', () => {
      passwordControl.setValue('');

      expect(passwordControl.valid).toBeFalse();
      expect(passwordControl?.errors?.['required']).toBeTrue();
    });

    it('should be invalid when less than 8 characters', () => {
      passwordControl.setValue('1234567');

      expect(passwordControl.valid).toBeFalse();
      expect(passwordControl?.errors?.['minlength']).toBeTruthy();
    });
  });
});
