import { Component, OnInit } from '@angular/core';
import { Product, ProductImage } from '../../models/product.model';
import { ProductsService } from '../../services/products.service';
import { ActivatedRoute } from '@angular/router';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
  standalone: false
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  selectedSize: string = '';
  selectedColor: string = '';
  quantity: number = 1;
  availableSizes: string[] = [];
  availableColors: string[] = [];

  constructor(
    private productsService: ProductsService,
    private route: ActivatedRoute,
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productsService.getProduct(+id).subscribe(product => {
        if (product) {
          this.product = product;

          this.updateSizeColorOptions();
        }
      });
    }
  }

  updateSizeColorOptions(): void {
    const variants = this.product.variants;

    this.availableSizes = [...new Set(variants.map(v => v.size))];
    this.selectedSize = this.availableSizes[0] || '';

    this.updateColorsForSelectedSize();
  }

  sortedImages(): ProductImage[] {
    if (!this.product || !this.product.images) return [];
    return [...this.product.images].sort((a, b) => a.order - b.order);
  }

  updateColorsForSelectedSize(): void {
    const variants = this.product.variants.filter(v => v.size === this.selectedSize);
    this.availableColors = [...new Set(variants.map(v => v.color))];

    // Set selectedColor to the first available, or clear it if invalid
    if (!this.availableColors.includes(this.selectedColor)) {
      this.selectedColor = this.availableColors[0] || '';
    }
  }

  updateSizesForSelectedColor(): void {
    const variants = this.product.variants.filter(v => v.color === this.selectedColor);
    this.availableSizes = [...new Set(variants.map(v => v.size))];

    if (!this.availableSizes.includes(this.selectedSize)) {
      this.selectedSize = this.availableSizes[0] || '';
    }
  }

  onSizeSelected(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.selectedSize = value;
    this.updateColorsForSelectedSize();
  }
  
  onColorSelected(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.selectedColor = value;
    this.updateSizesForSelectedColor();
  }  

  addToCart(): void {
    const variant = this.product.variants.find(v =>
      v.size === this.selectedSize && v.color === this.selectedColor
    );
  
    if (!variant) return;
  
    this.cartService.addToCart(this.product, this.quantity, variant);
  }

  get mainImageUrl(): string {
    if (!this.product || !this.product.images) return '';
    const mainImg = this.product.images.find(img => img.isMain);
    return mainImg ? mainImg.imageUrl : this.product.images[0]?.imageUrl || '';
  }

  getFillPercentage(star: number): number {
    const rating = this.product?.rating ?? 0;
    return Math.max(0, Math.min(1, rating - star + 1)) * 100;
  }

  getStarPath(star: number): string {
    return this.getFillPercentage(star) > 0 ?
      'M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z' :
      '';
  }

  increaseQuantity(): void {
    if (this.quantity < 10) this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) this.quantity--;
  }

  get selectedVariant() {
    return this.product?.variants.find(v =>
      v.size === this.selectedSize && v.color === this.selectedColor
    );
  }

  get isInStock(): boolean {
    return !!this.selectedVariant && this.selectedVariant.stock > 0;
  }  
  
}
