import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import {
  NgbCollapseModule,
  NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './header.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { User } from '../../contracts/user';
import { EnglishLevel } from '../../enums/englishLevel';
import { Observable, of } from 'rxjs';
import { HttpClientModule } from '@angular/common/http';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let authService: AuthenticationService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HeaderComponent],
      imports: [
        RouterTestingModule,
        NgbCollapseModule,
        NgbDropdownModule,
        HttpClientModule,
      ],
      providers: [],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthenticationService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize isCollapsed to true', () => {
    expect(component.isCollapsed).toBeTrue();
  });

  it('should initialize user to null', () => {
    expect(component.user).toBeNull();
  });

  it('should call authService.logout() when logout is called', () => {
    spyOn(authService, 'logout');
    component.logout();
    expect(authService.logout).toHaveBeenCalled();
  });

  describe('when user is authenticated', () => {
    const user: User = {
      id: 1,
      userName: 'jogndoe',
      englishLevel: EnglishLevel.Elementary,
      imageUrl: 'https://example.com/avatar.png',
    };
    beforeEach(() => {
      spyOn<AuthenticationService, any>(
        authService,
        'currentUser$' as unknown as Observable<User | null>
      ).and.returnValue(of(user));
      component.user = user;
      fixture.detectChanges();
    });

    it('should set user to the authenticated user', () => {
      expect(component.user).toEqual(user);
    });

    it('should not show the unauthorizedBlock', () => {
      const unauthorizedBlock =
        fixture.nativeElement.querySelector('#unauthorizedBlock');
      expect(unauthorizedBlock).toBeNull();
    });

    it('should show the achievements link', () => {
      const achievementsLink = fixture.nativeElement.querySelector(
        'a[routerLink="/achievements"]'
      );
      expect(achievementsLink).not.toBeNull();
    });

    it('should show the account settings and log out links in the dropdown', () => {
      const dropdownToggle = fixture.nativeElement.querySelector(
        'button[ngbDropdownToggle]'
      );
      dropdownToggle.click();
      fixture.detectChanges();
      const accountLink = fixture.nativeElement.querySelector(
        'a[routerLink="/account"]'
      );
      const logoutLink = fixture.nativeElement.querySelector(
        'a[routerLink="/login"]'
      );
      expect(accountLink).not.toBeNull();
      expect(logoutLink).not.toBeNull();
    });
  });

  describe('when user is not authenticated', () => {
    beforeEach(() => {
      spyOn<AuthenticationService, any>(
        authService,
        'currentUser$'
      ).and.returnValue(of(null));
      component.user = null;
      fixture.detectChanges();
    });

    it('should set user to null', () => {
      expect(component.user).toBeNull();
    });

    it('should show the unauthorizedBlock', () => {
      const unauthorizedBlock = fixture.nativeElement.querySelector(
        '#navbarContentUnauthorized'
      );
      expect(unauthorizedBlock).toBeTruthy();
    });

    it('should not show the achievements link', () => {
      const achievementsLink = fixture.nativeElement.querySelector(
        'a[routerLink="/achievements"]'
      );
      expect(achievementsLink).toBeNull();
    });
  });
});
