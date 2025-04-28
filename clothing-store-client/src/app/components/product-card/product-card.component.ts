import { Component, Input, OnInit } from '@angular/core';
import { Product, ProductImage, ProductVariant } from '../../models/product.model';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
  standalone: false
})
export class ProductCardComponent implements OnInit {
  @Input() product!: Product;

  selectedSize!: string;
  selectedColor!: string;
  selectedVariant?: ProductVariant;

  constructor(
    private cartService: CartService,
    private router: Router,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.setInitialSelection();
  }

  setInitialSelection(): void {
    const first = this.product.variants?.[0];
    if (first) {
      this.selectedSize = first.size;
      this.selectedColor = first.color;
      this.updateSelectedVariant();
    }
  }

  availableSizes(): string[] {
    return [...new Set(this.product.variants.map(v => v.size))];
  }

  availableColors(): string[] {
    return [...new Set(
      this.product.variants
        .filter(v => v.size === this.selectedSize)
        .map(v => v.color)
    )];
  }

  updateSelectedVariant(): void {
    this.selectedVariant = this.product.variants.find(v =>
      v.size === this.selectedSize && v.color === this.selectedColor
    );
  }

  getFillPercentage(star: number): number {
    if (!this.product || this.product.rating === undefined) return 0;
    const fill = Math.max(0, Math.min(1, this.product.rating - star + 1)) * 100;
    return fill;
  }

  getStarPath(star: number): string {
    return this.getFillPercentage(star) > 0
      ? 'M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z'
      : '';
  }

  sortedImages(): ProductImage[] {
    return [...this.product.images].sort((a, b) => a.order - b.order);
  }

  addToCart(): void {
    this.accountService.isAuthenticated$.pipe(take(1)).subscribe(isAuth => {
      if (!isAuth) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: `/products` } });
      } else if (this.selectedVariant && this.selectedVariant.stock > 0) {
        this.cartService.addToCart(this.product, 1, this.selectedVariant);
      }
    });
  }

  get isInStock(): boolean {
    return !!this.selectedVariant && this.selectedVariant.stock > 0;
  }  
}
