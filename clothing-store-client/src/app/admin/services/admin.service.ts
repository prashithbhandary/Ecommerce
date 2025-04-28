import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Category } from '../../models/Category';
import { AccountService } from '../../services/account.service';
import { User } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = `${environment.apiUrl}`; // Add to your environment.ts

  constructor(private http: HttpClient, private accountService: AccountService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Category Methods
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/category/getallcategory`,{ headers: this.getAuthHeaders() });
  }

  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/category/getcategorybyid/${id}`, { headers: this.getAuthHeaders() });
  }

  createCategory(categoryData: Omit<Category, 'id'>): Observable<Category> {
    return this.http.post<Category>(`${this.apiUrl}/category/addcategory`, categoryData, { headers: this.getAuthHeaders() });
  }

  updateCategory(categoryData: Category): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/category/updatecategory`, categoryData, { headers: this.getAuthHeaders() });
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/category/deletecategory/${id}`, { headers: this.getAuthHeaders() });
  }

  // User Management Methods
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/account/all-user`, { headers: this.getAuthHeaders() });
  }

  getUserByEmail(email: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/account/user-by-email`, { headers: this.getAuthHeaders() });
  }
  
  deleteUser(email: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/account/delete-user/${email}`, { headers: this.getAuthHeaders() });
  }

  updateUser(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/account/update/${data}`, { headers: this.getAuthHeaders() });
  }

  createUser(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/account/admin-create-user`, data, { headers: this.getAuthHeaders() });
  }

  // Add other admin methods as needed
}