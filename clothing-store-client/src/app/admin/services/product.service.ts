import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { AccountService } from '../../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = `${environment.apiUrl}/product`;

  constructor(private http: HttpClient, private accountService: AccountService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getAll() {
    return this.http.get<any[]>(`${this.baseUrl}/get-all-product`);
  }

  createProduct(formData: FormData) {
    return this.http.post(`${this.baseUrl}/create-product`, formData, { headers: this.getAuthHeaders() });
  }

  getProductById(id: number) {
    return this.http.get<any>(`${this.baseUrl}/product-by-id/${id}`);
  }
  
  updateProduct(id: number, formData: FormData) {
    return this.http.put(`${this.baseUrl}/update-product/${id}`, formData, {
      headers: this.getAuthHeaders()
    });
  }

  // You can add create, update, delete methods here as needed
}
