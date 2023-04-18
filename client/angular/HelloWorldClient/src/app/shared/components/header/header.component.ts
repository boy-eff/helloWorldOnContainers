import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserModel } from '../../contracts/userModel';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  public isCollapsed = true;
  public user: UserModel | null = null;

  constructor(public authService: AuthenticationService) {}
  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.user = user;
    });
  }

  logout() {
    this.authService.logout();
  }
}
