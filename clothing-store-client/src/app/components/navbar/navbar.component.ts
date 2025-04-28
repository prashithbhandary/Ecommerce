import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { catchError, Observable, of } from 'rxjs';
import { ProductsService } from '../../services/products.service';
import { AccountService } from '../../services/account.service';
import { CategoryService } from '../../services/CategoryService';
import { Category } from '../../models/Category';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  standalone: false
})
export class NavbarComponent implements OnInit {
  cartItemCount$: Observable<number>;
  isAuthenticated$!: Observable<boolean>;
  categories$!: Observable<Category[]>;
  isAdmin = false;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private productsService: ProductsService,
    private accountService: AccountService,
    private categoryService: CategoryService
  ) {
    this.cartItemCount$ = this.cartService.getTotalItems();
  }

  ngOnInit(): void {
    this.isAuthenticated$ = this.accountService.isAuthenticated$;
    this.accountService.isAdmin$.subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });
    console.log(this.isAdmin);
    this.loadCategories();
  }

  logout(): void {
    this.accountService.logout();
  }

  private loadCategories(): void {
    this.categories$ = this.categoryService.getAllCategories().pipe(
      catchError(error => {
        console.error('Error loading categories: ', error);
        return of([]); // Return empty array as fallback
      })
    );
  }
}