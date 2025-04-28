import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item.model';
import { Observable } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
  standalone: false
})
export class CartComponent implements OnInit {
  cartItems$: Observable<CartItem[]>;
  totalAmount$: Observable<number>;
  isAuthenticated: boolean = false;

  constructor(
    private cartService: CartService,
    private accountService: AccountService
  ) {
    this.cartItems$ = this.cartService.getCartItems();
    this.totalAmount$ = this.cartService.getTotalAmount();
  }

  ngOnInit(): void {
    this.accountService.isAuthenticated$.subscribe(status => {
      this.isAuthenticated = status;
    });
  }

  removeItem(item: CartItem): void {
    this.cartService.removeFromCart(item);
  }

  updateQuantity(item: CartItem, quantity: number): void {
    this.cartService.updateQuantity(item, quantity);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }

  getMainImageUrl(item: CartItem): string {
    const mainImage = item.product?.images?.find(img => img.isMain);
    return mainImage?.imageUrl || 'assets/images/placeholder.png';
  }
}
