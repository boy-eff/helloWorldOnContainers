import { Component } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  public isCollapsed = true;

  constructor(public authService: AuthenticationService) {

  }

  logout() {
    console.log(13);
    this.authService.logout();
  }
}
