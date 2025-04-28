import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-address-form',
  standalone: false,
  templateUrl: './address-form.component.html',
  styleUrl: './address-form.component.scss'
})
export class AddressFormComponent implements OnInit {
  addressForm: FormGroup;
  isEditMode = false;
  addressId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.addressForm = this.fb.group({
      name:['',Validators.required],
      addressLine1: ['', Validators.required],
      addressLine2: [''],
      city: ['', Validators.required],
      state: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
      isPrimary: [false],
    });
  }

  ngOnInit(): void {
    this.addressId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.addressId) {
      this.isEditMode = true;
      this.accountService.getAddressById(this.addressId).subscribe(address => {
        this.addressForm.patchValue(address);
      });
    }
  }

  onSubmit(): void {
    if (this.addressForm.invalid) return;
    const token = this.accountService.getToken();
    if (!token) return;
  
    const decoded: any = this.accountService.decodeToken(token);
    if (!decoded || !decoded.sub) {
      this.toastr.error('User ID not found in token');
      return;
    }
  
    const userId = parseInt(decoded.sub); // <- User ID from JWT
  
    const addressData = {
      ...this.addressForm.value,
      userId: userId
    };
    if (this.isEditMode && this.addressId) {
      addressData.id = this.addressId;
    }
    
    const operation = this.isEditMode && this.addressId
      ? this.accountService.updateAddress(addressData)
      : this.accountService.addAddress(addressData);
  
    operation.subscribe({
      next: () => {
        const action = this.isEditMode ? 'updated' : 'created';
        this.toastr.success(`Address ${action} successfully`);
        this.router.navigate(['/addresses']);
      },
      error: (err) => {
        const action = this.isEditMode ? 'update' : 'create';
        this.toastr.error(`Failed to ${action} address`);
        console.error('Operation error:', err);
      }
    });
  }
}
