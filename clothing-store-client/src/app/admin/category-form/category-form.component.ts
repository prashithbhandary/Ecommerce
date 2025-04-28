import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from '../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../models/Category';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.scss'],
  standalone: false
})
export class CategoryFormComponent implements OnInit {
  categoryForm: FormGroup;
  categoryId: number | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.categoryId = +params['id'];
        this.loadCategory(this.categoryId);
      }
    });
  }

  loadCategory(id: number): void {
    this.isLoading = true;
    this.adminService.getCategoryById(id).subscribe({
      next: (category: Category) => {
        this.categoryForm.patchValue({
          name: category.name,
          description: category.description || ''
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.toastr.error('Failed to load category');
        console.error('Error loading category:', err);
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const categoryData = this.categoryForm.value;

    const operation = this.categoryId 
      ? this.adminService.updateCategory({ 
          id: this.categoryId, 
          name: categoryData.name, 
          description: categoryData.description 
        })
      : this.adminService.createCategory({
          name: categoryData.name,
          description: categoryData.description
        });

    operation.subscribe({
      next: () => {
        this.toastr.success(`Category ${this.categoryId ? 'updated' : 'created'} successfully`);
        this.router.navigate(['/admin/categories']);
      },
      error: (err) => {
        this.toastr.error(`Failed to ${this.categoryId ? 'update' : 'create'} category`);
        console.error('Operation error:', err);
        this.isLoading = false;
      }
    });
  }
}