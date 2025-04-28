import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../services/products.service';
import { Product } from '../../models/product.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  standalone: false
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  isLoading = true;

  constructor(
    private productsService: ProductsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.isLoading = true;
      const category = params['category'];
      const searchTerm = params['search'];

      let request$;

      if (category) {
        request$ = this.productsService.getProductsByCategory(category);
      } else if (searchTerm) {
        request$ = this.productsService.searchProducts(searchTerm);
      } else {
        request$ = this.productsService.getProducts();
      }

      request$.subscribe({
        next: (products: Product[]) => {
          this.products = products;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error fetching products:', error);
          this.products = [];
          this.isLoading = false;
        }
      });
    });
  }
}