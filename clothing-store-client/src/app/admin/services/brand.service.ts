import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from '../../services/account.service';
import { Observable } from 'rxjs';
import { brand } from '../../models/brand.model';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  private apiUrl = `${environment.apiUrl}`; // Add to your environment.ts

  constructor(private http: HttpClient, private accountService: AccountService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getToken();
    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  // Category Methods
  getBrands(): Observable<brand[]> {
    return this.http.get<brand[]>(`${this.apiUrl}/brand/getallbrand`,{ headers: this.getAuthHeaders() });
  }

  getBrandById(id: number): Observable<brand> {
    return this.http.get<brand>(`${this.apiUrl}/brand/getbrandbyid/${id}`, { headers: this.getAuthHeaders() });
  }

  createBrand(brandData: Omit<brand, 'id'>): Observable<brand> {
    return this.http.post<brand>(`${this.apiUrl}/brand/addbrand`, brandData, { headers: this.getAuthHeaders() });
  }

  updateBrand(brandData: brand): Observable<brand> {
    return this.http.put<brand>(`${this.apiUrl}/brand/updatebrand`, brandData, { headers: this.getAuthHeaders() });
  }

  deleteBrand(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/brand/deletebrand/${id}`, { headers: this.getAuthHeaders() });
  }
}
