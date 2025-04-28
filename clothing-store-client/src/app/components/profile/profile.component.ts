import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  email!: string;
  userId!: number;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadUser();
  }

  initForm() {
    this.profileForm = this.fb.group({
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
      phoneNumber: [''],
    });
  }

  loadUser() {
    const token = this.accountService.getToken();
    if (!token) return;

    const decoded: any = this.accountService.decodeToken(token);
    if (decoded) {
      this.email = decoded.email;

      this.accountService.getUserByEmail(this.email).subscribe(user => {
        this.userId = user.id;
        this.profileForm.patchValue({
          email: user.email,
          fullName: user.fullName,
          phoneNumber: user.phoneNumber
        });
      });
    } else {
      console.error('Invalid token or failed to decode');
    }
  }

  onSubmit() {
    if (this.profileForm.invalid) return;

    const formValues = this.profileForm.getRawValue();

    const updatedUser = {
      id: this.userId,
      email: formValues.email,
      fullName: formValues.fullName,
      phoneNumber: formValues.phoneNumber
    };

    this.accountService.updateProfile(updatedUser).subscribe({
      next: () => {
        this.toastr.success('Profile updated successfully!');
        this.router.navigate(['/profile']);
      },
      error: (err) => {
        this.toastr.error('Failed to update profile');
        console.error('Profile update error:', err);
      }
    });
  }
}
