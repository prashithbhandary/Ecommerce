import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { AccountService } from './account.service';
import { CartItem } from '../models/cart-item.model';

interface CreateOrderResponse {
  orderId: string;
}

interface VerifyPaymentRequest {
  orderId: string;
  paymentId: string;
  signature: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private baseUrl = `${environment.apiUrl}/payment`; // Change if different port

  constructor(private http: HttpClient, private accountService: AccountService) {}

  createOrder(amount: number, addressId: number | any, cartItems: CartItem[]): Observable<CreateOrderResponse> {
    const token = this.accountService.getToken();
    if (!token) throw new Error('User not logged in');
  
    const decoded: any = this.accountService.decodeToken(token);
    const userId = parseInt(decoded.sub);
  
    const body = {
      amount: amount,
      currency: 'INR',
      receipt: 'receipt_' + new Date().getTime(),
      userId: userId,
      addressId: addressId,
      cartItems: cartItems.map(item => ({
        productName: item.product.name,
        productId: item.product.id,
        productVariantId: item.variant?.id || null,  // ✅ Variant Id or null
        quantity: item.quantity,
        price: item.product.price  // ✅ Pick correct price
      }))
    };
  
    return this.http.post<CreateOrderResponse>(`${this.baseUrl}/create-order`, body);
  }

  verifyPayment(data: VerifyPaymentRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/verify`, data);
  }

  sendOrderConfirmationEmail(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/send-order-confirmation`, data);
  }  

  placeCodOrder(amount: number, addressId: number | any, cartItems: CartItem[]): Observable<any> {
    const token = this.accountService.getToken();
    if (!token) throw new Error('User not logged in');
  
    const decoded: any = this.accountService.decodeToken(token);
    const userId = parseInt(decoded.sub);
  
    const body = {
      amount: amount,
      currency: 'INR',
      userId: userId,
      addressId: addressId,
      cartItems: cartItems.map(item => ({
        productName: item.product.name,
        productId: item.product.id,
        productVariantId: item.variant?.id || null,
        quantity: item.quantity,
        price: item.product.price
      }))
    };
  
    return this.http.post(`${this.baseUrl}/cod-order`, body);
  }
  
}
