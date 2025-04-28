import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-admin-navbar',
  standalone: false,
  templateUrl: './admin-navbar.component.html',
  styleUrl: './admin-navbar.component.scss'
})
export class AdminNavbarComponent {
  constructor(private accountService: AccountService) {}

  logout() {
    this.accountService.logout();
  }
}
