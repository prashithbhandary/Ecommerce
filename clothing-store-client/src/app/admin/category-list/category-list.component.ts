import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Category } from '../../models/Category';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss'], // Optional styling
  standalone: false
})
export class CategoryListComponent implements OnInit {
  categories$!: Observable<Category[]>;
  isLoading = false;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.categories$ = this.adminService.getCategories();
    this.categories$.subscribe({
      next: () => this.isLoading = false,
      error: () => {
        this.toastr.error('Failed to load categories');
        this.isLoading = false;
      }
    });
  }

  deleteCategory(id: number): void {
    if (confirm('Are you sure you want to delete this category?')) {
      this.isLoading = true;
      this.adminService.deleteCategory(id).subscribe({
        next: () => {
          this.toastr.success('Category deleted successfully');
          this.loadCategories();
        },
        error: (err) => {
          this.toastr.error('Failed to delete category');
          console.error('Delete error:', err);
          this.isLoading = false;
        }
      });
    }
  }
}