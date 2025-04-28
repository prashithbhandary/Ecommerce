import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { CartItem } from '../models/cart-item.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { Product } from '../models/product.model';
import { ProductVariant } from '../models/ProductVariant.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems: CartItem[] = [];
  private cartSubject = new BehaviorSubject<CartItem[]>([]);
  private totalItemsSubject = new BehaviorSubject<number>(0);
  private totalAmountSubject = new BehaviorSubject<number>(0);
  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      this.loadCart();
    }
  }

  getCartItems(): Observable<CartItem[]> {
    return this.cartSubject.asObservable();
  }

  getTotalItems(): Observable<number> {
    return this.totalItemsSubject.asObservable();
  }

  getTotalAmount(): Observable<number> {
    return this.totalAmountSubject.asObservable();
  }

  addToCart(product: Product, quantity: number = 1, variant?: ProductVariant): void {
    const existingItem = this.cartItems.find(item =>
      item.product.id === product.id &&
      item.variant?.id === variant?.id
    );
  
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      this.cartItems.push({ product, quantity, variant });
    }
  
    this.updateCart();
  }  

  removeFromCart(item: CartItem): void {
    const index = this.cartItems.indexOf(item);
    if (index > -1) {
      this.cartItems.splice(index, 1);
      this.updateCart();
    }
  }

  updateQuantity(item: CartItem, quantity: number): void {
    if (quantity < 1) return;
    const existingItem = this.cartItems.find(cartItem =>
      cartItem.product.id === item.product.id &&
      cartItem.variant?.id === item.variant?.id
    );

    if (existingItem) {
      existingItem.quantity = quantity;
      this.updateCart();
    }
  }

  clearCart(): void {
    this.cartItems = [];
    this.updateCart();
  }

  private updateCart(): void {
    if (!this.isBrowser) return;
    this.cartSubject.next([...this.cartItems]);
    this.updateTotals();
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }

  private loadCart(): void {
    if (!this.isBrowser) return;
    const saved = localStorage.getItem('cart');
    if (saved) {
      this.cartItems = JSON.parse(saved);
      this.updateCart();
    }
  }

  private updateTotals(): void {
    const totalItems = this.cartItems.reduce((sum, item) => sum + item.quantity, 0);
    const totalAmount = this.cartItems.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);
    
    this.totalItemsSubject.next(totalItems);
    this.totalAmountSubject.next(parseFloat(totalAmount.toFixed(2)));
  }
}