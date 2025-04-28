import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { brand } from '../../../models/brand.model';
import { AdminService } from '../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { BrandService } from '../../services/brand.service';

@Component({
  selector: 'app-brand-list',
  standalone: false,
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.scss'
})
export class BrandListComponent implements OnInit {
  brands$!: Observable<brand[]>;
  isLoading = false;

  constructor(
    private brandService: BrandService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadBrands();
  }

  loadBrands(): void {
    this.isLoading = true;
    this.brands$ = this.brandService.getBrands();
    this.brands$.subscribe({
      next: () => this.isLoading = false,
      error: () => {
        this.toastr.error('Failed to load brands');
        this.isLoading = false;
      }
    });
  }

  deleteBrand(id: number): void {
    if (confirm('Are you sure you want to delete this brand?')) {
      this.isLoading = true;
      this.brandService.deleteBrand(id).subscribe({
        next: () => {
          this.toastr.success('Brand deleted successfully');
          this.loadBrands();
        },
        error: (err) => {
          this.toastr.error('Failed to delete brand');
          console.error('Delete error:', err);
          this.isLoading = false;
        }
      });
    }
  }
}
