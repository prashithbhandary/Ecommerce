import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-user-form',
  standalone: false,
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.scss'
})
export class UserFormComponent implements OnInit {
  userForm!: FormGroup;
  isLoading = false;
  userEmail: string | null = null;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const stateUser = history.state?.user as User;
  
    this.userForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [null, this.userEmail ? [] : [Validators.required, Validators.minLength(6)]],
      phoneNumber: ['', Validators.required],
      isAdmin: [false]
    });
  
    this.route.params.subscribe(params => {
      const email = params['email'];
  
      if (email) {
        this.userEmail = email;
  
        if (stateUser && stateUser.email === email) {
          this.patchUserForm(stateUser);
        } else {
          this.loadUserFromApi(email);
        }
      }
    });
  }
  

  patchUserForm(user: User): void {
    this.userForm.patchValue({
      fullName: user.fullName,
      email: user.email,
      phoneNumber: user.phoneNumber,
      isAdmin: user.isAdmin
    });
    this.userForm.get('email')?.disable();
  }
  
  loadUserFromApi(email: string): void {
    this.isLoading = true;
    this.adminService.getUserByEmail(email).subscribe({
      next: user => {
        this.patchUserForm(user);
        this.isLoading = false;
      },
      error: () => {
        this.toastr.error('User not found');
        this.router.navigate(['/admin/users']);
      }
    });
  }
  

  onSubmit(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const userData = this.userForm.getRawValue();

    const request = this.userEmail
      ? this.adminService.updateUser(userData)
      : this.adminService.createUser(userData);

    request.subscribe({
      next: () => {
        const action = this.userEmail ? 'updated' : 'created';
        this.toastr.success(`User ${action} successfully`);
        this.router.navigate(['/admin/users']);
      },
      error: (err) => {
        this.toastr.error('Failed to save user');
        console.error('Submit error:', err);
        this.isLoading = false;
      }
    });
  }
}