import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';
import { EnglishLevelOption } from 'src/app/shared/contracts/englishLevelOption';
import { RegisterComponent } from './register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';
import { User } from 'src/app/shared/contracts/user';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let usersServiceSpy: jasmine.SpyObj<UsersService>;
  let routerSpy: jasmine.SpyObj<Router>;

  const testUsername = 'testuser';
  const testPassword = 'testpassword';
  const testConfirmPassword = 'testpassword';
  const testEnglishLevel: EnglishLevelOption = {
    value: EnglishLevel.Elementary,
    displayValue: EnglishLevel.Elementary.toString(),
  };
  const mockUser: User = {
    id: 123,
    englishLevel: testEnglishLevel.value,
    userName: testUsername,
    imageUrl: '',
  };

  beforeEach(async () => {
    usersServiceSpy = jasmine.createSpyObj('UsersService', ['registerUser']);
    routerSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      declarations: [RegisterComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: UsersService, useValue: usersServiceSpy },
        { provide: Router, useValue: routerSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('registerForm', () => {
    it('should be invalid when empty', () => {
      component.registerForm.setValue({
        username: '',
        password: '',
        confirmPassword: '',
        englishLevel: '',
      });
      expect(component.registerForm.valid).toBeFalse();
    });

    it('should be invalid when username is too short', () => {
      component.registerForm.setValue({
        username: 'test',
        password: testPassword,
        confirmPassword: testConfirmPassword,
        englishLevel: testEnglishLevel.value,
      });
      expect(component.registerForm.valid).toBeFalse();
    });

    it('should be invalid when password is too short', () => {
      component.registerForm.setValue({
        username: testUsername,
        password: 'test',
        confirmPassword: testConfirmPassword,
        englishLevel: testEnglishLevel.value,
      });
      expect(component.registerForm.valid).toBeFalse();
    });

    it('should be invalid when password and confirm password do not match', () => {
      component.registerForm.setValue({
        username: testUsername,
        password: testPassword,
        confirmPassword: 'notthesame',
        englishLevel: testEnglishLevel.value,
      });
      expect(component.registerForm.valid).toBeFalse();
    });

    it('should be valid when all fields are filled out correctly', () => {
      component.registerForm.setValue({
        username: testUsername,
        password: testPassword,
        confirmPassword: testConfirmPassword,
        englishLevel: testEnglishLevel.value,
      });
      expect(component.registerForm.valid).toBeTrue();
    });
  });

  describe('onSubmit', () => {
    it('should call usersService.registerUser with form value', () => {
      const response = {};
      usersServiceSpy.registerUser.and.returnValue(of(mockUser));
      component.registerForm.setValue({
        username: testUsername,
        password: testPassword,
        confirmPassword: testConfirmPassword,
        englishLevel: testEnglishLevel.value,
      });

      component.onSubmit();

      expect(usersServiceSpy.registerUser).toHaveBeenCalledWith({
        username: testUsername,
        password: testPassword,
        englishLevel: testEnglishLevel.value,
      });
      expect(routerSpy.navigateByUrl).toHaveBeenCalledWith('/login');
    });

    it('should not call UsersService.registerUser and display error message on unsuccessful registration', () => {
      const errorResponse = new HttpErrorResponse({
        status: HttpStatusCode.Conflict,
        error: { message: 'Username already exists' },
      });
      usersServiceSpy.registerUser.and.returnValue(
        throwError(() => errorResponse)
      );
      component.registerForm.setValue({
        username: testUsername,
        password: testPassword,
        confirmPassword: testConfirmPassword,
        englishLevel: testEnglishLevel.value,
      });

      component.onSubmit();

      expect(usersServiceSpy.registerUser).toHaveBeenCalledWith({
        username: testUsername,
        password: testPassword,
        englishLevel: testEnglishLevel.value,
      });
      expect(component.conflictResponse).toBeTrue();
    });
  });
});
