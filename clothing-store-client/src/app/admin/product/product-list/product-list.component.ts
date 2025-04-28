import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-list',
  standalone: false,
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  products: any[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getAll().subscribe({
      next: (res) => this.products = res,
      error: (err) => console.error('Failed to load products', err)
    });
  }

  editProduct(product: any) {
    // Route to an edit page or open modal
    console.log('Edit product', product);
  }

  getMainImage(product: any): string {
    const mainImg = product.images?.find((img: any) => img.isMain);
    return mainImg ? mainImg.imageUrl : 'https://via.placeholder.com/300';
  }
}