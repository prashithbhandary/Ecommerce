import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../../services/products.service';
import { Router } from '@angular/router';
import { CategoryService } from '../../services/CategoryService';

@Component({
  selector: 'app-category-filter',
  templateUrl: './category-filter.component.html',
  styleUrls: ['./category-filter.component.scss'],
  standalone: false
})
export class CategoryFilterComponent implements OnInit {
  categories$ = this.categoryService.getAllCategories();

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  filterByCategory(category: string): void {
    this.router.navigate(['/products'], { queryParams: { category } });
  }

  clearFilters(): void {
    this.router.navigate(['/products']);
  }
}