import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { AdminOrder, AdminOrderDetail, OrderDetail } from '../models/Order.model';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = `${environment.apiUrl}/order`;

  constructor(private http: HttpClient, private accountService: AccountService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Fetch the list of orders for the authenticated user
  getOrders(): Observable<any[]> {
    const token = this.accountService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    return this.http.get<any[]>(`${this.baseUrl}/user-orders`, { headers });
  }

  // Fetch order details for a specific order ID
  getOrderDetails(orderId: number): Observable<OrderDetail> {
    const token = this.accountService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    return this.http.get<OrderDetail>(`${this.baseUrl}/order-details/${orderId}`, { headers });
  }

  getOrdersAdmin(status?: string): Observable<AdminOrder[]> {
    let params = new HttpParams();
    if (status) {
      params = params.append('status', status);
    }
    return this.http.get<AdminOrder[]>(`${this.baseUrl}/admin-orders`, { params, headers: this.getAuthHeaders()  });
  }

  getOrderDetailsAdmin(orderId: number): Observable<AdminOrderDetail> {
    return this.http.get<AdminOrderDetail>(`${this.baseUrl}/admin-order-details/${orderId}`,{ headers: this.getAuthHeaders() });
  }

}
