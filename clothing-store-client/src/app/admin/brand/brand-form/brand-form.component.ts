import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrandService } from '../../services/brand.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { brand } from '../../../models/brand.model';

@Component({
  selector: 'app-brand-form',
  standalone: false,
  templateUrl: './brand-form.component.html',
  styleUrl: './brand-form.component.scss'
})
export class BrandFormComponent implements OnInit {
  brandForm: FormGroup;
  brandId: number | null = null;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private brandService: BrandService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.brandForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.brandId = +params['id'];
        this.loadBrand(this.brandId);
      }
    });
  }

  loadBrand(id: number): void {
    this.isLoading = true;
    this.brandService.getBrandById(id).subscribe({
      next: (brand: brand) => {
        this.brandForm.patchValue({
          name: brand.name,
          description: brand.description || ''
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.toastr.error('Failed to load brand');
        console.error('Error loading brand:', err);
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.brandForm.invalid) {
      this.brandForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const brandData = this.brandForm.value;

    const operation = this.brandId
      ? this.brandService.updateBrand({
          id: this.brandId,
          name: brandData.name,
          description: brandData.description
        })
      : this.brandService.createBrand({
          name: brandData.name,
          description: brandData.description
        });

    operation.subscribe({
      next: () => {
        this.toastr.success(`Brand ${this.brandId ? 'updated' : 'created'} successfully`);
        this.router.navigate(['/admin/brands']);
      },
      error: (err) => {
        this.toastr.error(`Failed to ${this.brandId ? 'update' : 'create'} brand`);
        console.error('Operation error:', err);
        this.isLoading = false;
      }
    });
  }
}
