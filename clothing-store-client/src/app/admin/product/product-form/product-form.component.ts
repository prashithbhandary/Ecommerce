import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { AdminService } from '../../services/admin.service';
import { BrandService } from '../../services/brand.service';
import { brand } from '../../../models/brand.model';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-product-form',
  standalone: false,
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {
  product: any = {
    name: '',
    price: 0,
    stock: 0,
    description: '',
    brandId: null,
    categoryId: null,
    variants: []
  };

  productId: any | null = null;
  // productForm: FormGroup;
  categories: any[] = [];
  brands: brand[] = [];
  hasVariants = false;
  isLoading = false;

  imagePreviews: any[] = [];
  selectedImages: File[] = [];
  mainImageIndex: number = 0;
  existingImages: { id: number; url: string; isMain: boolean }[] = [];

  constructor(
    private productService: ProductService,
    private adminService: AdminService,
    private brandService: BrandService,
    private router: Router,
    private toastr: ToastrService,
    private route: ActivatedRoute
  ) {}

  async ngOnInit(): Promise<void> {
    await this.loadDropdownData();
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.productId = id;
      this.loadProduct(id);
    }
    console.log('Selected Brand ID:', this.product.brandId);
console.log('Selected Category ID:', this.product.categoryId);
  }

  loadDropdownData(): Promise<void> {
    return new Promise((resolve, reject) => {
      let count = 0;
      // Get categories
      this.adminService.getCategories().subscribe({
        next: (res) => {
          this.categories = res;
          if (++count === 2) resolve();
        },
        error: () => reject('Error loading categories')
      });
      // Get brands
      this.brandService.getBrands().subscribe({
        next: (res) => {
          this.brands = res;
          if (++count === 2) resolve();
        },
        error: () => reject('Error loading brands')
      });
    });
  }

  compareById(a: any, b: any): boolean {
    return a && b && a === b;
  }

  loadProduct(id: number): void {
    this.productService.getProductById(id).subscribe({
      next: (res) => {
  
        // Find the brand ID by matching the brand name from the API response
        const selectedBrand = this.brands.find(brand => brand.name === res.brandName);
        const selectedCategory = this.categories.find(category => category.name === res.categoryName);
  
        this.product = {
          name: res.name,
          price: res.price,
          stock: res.stock,
          description: res.description,
          brandId: selectedBrand ? selectedBrand.id : null,  // Map to brand ID
          categoryId: selectedCategory ? selectedCategory.id : null,  // Map to category ID
          variants: res.variants || []
        };
  
        this.hasVariants = res.variants && res.variants.length > 0;
  
        // Handle images for preview
        this.imagePreviews = res.images.map((img: any, index: number) => {
          if (img.isMain) this.mainImageIndex = index;
          return { url: img.imageUrl };
        });

        this.existingImages = res.images.map((img: any, index: number) => {
          if (img.isMain) this.mainImageIndex = index;
          return { id: img.id, url: img.imageUrl, isMain: img.isMain };
        });        
  
        this.selectedImages = [];  // Reset selected images
      },
      error: () => {
        this.toastr.error('Failed to load product');
      }
    });
  }
  

  onImageSelect(event: any) {
    const files = Array.from(event.target.files) as File[];
    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagePreviews.push({ url: e.target.result });
        this.selectedImages.push(file);
      };
      reader.readAsDataURL(file);
    });
  }

  moveImageUp(index: number) {
    if (index > 0) {
      [this.imagePreviews[index], this.imagePreviews[index - 1]] = [this.imagePreviews[index - 1], this.imagePreviews[index]];
      [this.selectedImages[index], this.selectedImages[index - 1]] = [this.selectedImages[index - 1], this.selectedImages[index]];
    }
  }

  moveImageDown(index: number) {
    if (index < this.imagePreviews.length - 1) {
      [this.imagePreviews[index], this.imagePreviews[index + 1]] = [this.imagePreviews[index + 1], this.imagePreviews[index]];
      [this.selectedImages[index], this.selectedImages[index + 1]] = [this.selectedImages[index + 1], this.selectedImages[index]];
    }
  }

  removeImage(index: number) {
    this.imagePreviews.splice(index, 1);
  
    if (index < this.existingImages.length) {
      this.existingImages.splice(index, 1);
    } else {
      const newImageIndex = index - this.existingImages.length;
      this.selectedImages.splice(newImageIndex, 1);
    }
  
    if (this.mainImageIndex === index) {
      this.mainImageIndex = 0;
    }
  }  
  

  addVariant() {
    this.product.variants.push({ size: '', color: '', stock: 0, sku: '' });
  }

  removeVariant(index: number) {
    this.product.variants.splice(index, 1);
  }

  submit(): void {
    if (!this.product.name || !this.product.price || !this.product.stock || !this.product.description || !this.product.brandId || !this.product.categoryId) {
      this.toastr.error('Please fill in all required fields');
      return;
    }
  
    this.isLoading = true;
  
    const formData = new FormData();
    formData.append('name', this.product.name);
    formData.append('price', this.product.price.toString());
    formData.append('stock', this.product.stock.toString());
    formData.append('description', this.product.description);
    formData.append('brandId', this.product.brandId);
    formData.append('categoryId', this.product.categoryId);
  
    // Add selected images to form data
    this.selectedImages.forEach((img, index) => {
      formData.append('images', img);
      formData.append('isMainList', index + this.existingImages.length === this.mainImageIndex ? 'true' : 'false');
      formData.append('imageOrders', (index + this.existingImages.length).toString());
    });
  
    this.existingImages.forEach((img, index) => {
      formData.append('ExistingImageUrls', img.url);
      formData.append('isMainList', index === this.mainImageIndex ? 'true' : 'false');
      formData.append('imageOrders', index.toString());
    });


  formData.append('hasVariants', this.hasVariants.toString());
  if (this.hasVariants) {
    formData.append('variantsJson', JSON.stringify(this.product.variants));
  }
  
    const request$ = this.productId
      ? this.productService.updateProduct(this.productId, formData)
      : this.productService.createProduct(formData);
  
    request$.subscribe({
      next: () => {
        this.toastr.success(`Product ${this.productId ? 'updated' : 'added'} successfully`);
        this.router.navigate(['/admin/products']);
      },
      error: (err) => {
        this.toastr.error(`Failed to ${this.productId ? 'update' : 'add'} product`);
        console.error('Error', err);
        this.isLoading = false;
      }
    });
  }
}