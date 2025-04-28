import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { CategoryService } from '../services/CategoryService';
import { CartService } from '../services/cart.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main-layout',
  standalone: false,
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent implements OnInit {
  isAuthenticated$!: Observable<boolean>;
  isAdmin = false;
  categories$ = this.categoryService.getAllCategories();
  //cartItemCount$ = this.cartService.cartItemCount$;

  constructor(
    private accountService: AccountService,
    private categoryService: CategoryService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.isAuthenticated$ = this.accountService.isAuthenticated$;
    this.accountService.isAdmin$.subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });
  }

  logout() {
    this.accountService.logout();
  }
}
