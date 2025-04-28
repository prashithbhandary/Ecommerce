import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/user.model';
import { AdminService } from '../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  standalone: false,
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  isLoading = false;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers(): void {
    this.isLoading = true;
    this.adminService.getUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.isLoading = false;
      },
      error: () => {
        this.toastr.error('Failed to load users');
        this.isLoading = false;
      }
    });
  }

  editUser(user: User): void {
    this.router.navigate(['/admin/users/edit', user.email], { state: { user } });
  }  

  deleteUser(email: string): void {
    if (!confirm('Are you sure you want to delete this user?')) return;
    this.isLoading = true;

    this.adminService.deleteUser(email).subscribe({
      next: () => {
        this.toastr.success('User deleted successfully');
        this.fetchUsers(); // Refresh list
      },
      error: () => {
        this.toastr.error('Failed to delete user');
        this.isLoading = false;
      }
    });
  }
}
