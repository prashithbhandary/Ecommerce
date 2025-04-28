import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { Address } from '../models/address.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private apiUrl = 'https://localhost:7067/api/account'; // Adjust API URL as needed
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  private isAdminSubject = new BehaviorSubject<boolean>(false);
isAdmin$ = this.isAdminSubject.asObservable();

  constructor(private http: HttpClient, private router: Router, @Inject(PLATFORM_ID) private platformId: object) {
    if (isPlatformBrowser(this.platformId)) {
      this.isAuthenticatedSubject.next(this.hasToken()); // ✅ Only run this in the browser
      this.checkAdminStatus();
    }
  }

  private hasToken(): boolean {
    return isPlatformBrowser(this.platformId) ? !!localStorage.getItem('token') : false; // ✅ Check only in browser
  }

  isAdmin(): Observable<boolean> {
    return this.isAdmin$.pipe(take(1));
  }

  private checkAdminStatus(): void {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.decodeToken(token);
      console.log('Decoded Token:', decodedToken);
      const isAdminValue = decodedToken?.isAdmin;
    this.isAdminSubject.next(isAdminValue === true || isAdminValue === "True");
    }
  }

  login(credentials: { email: string, password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response.token && isPlatformBrowser(this.platformId)) {
          localStorage.setItem('token', response.token);
          this.isAuthenticatedSubject.next(true);
          
          // Decode token to check admin status
          this.checkAdminStatus();
        }
      })
    );
  }

  getUserByEmail(email: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/user-by-email`, {
      params: { email }
    });
  }
  
  updateProfile(user: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, user);
  }  

  decodeToken(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user).pipe(
      catchError(error => {

        console.error('Registration error:', error);
        return throwError(() => error);
      }));
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
    }
    this.isAuthenticatedSubject.next(false);
    this.isAdminSubject.next(false);
    this.router.navigate(['/']); // ✅ Redirect here
  }

  getToken(): string | null {
    return isPlatformBrowser(this.platformId) ? localStorage.getItem('token') : null;
  }

  private isTokenExpired(token: string): boolean {
    const decoded = this.decodeToken(token);
    if (!decoded?.exp) return true;
    const expiry = decoded.exp * 1000;
    return Date.now() > expiry;
  }
  
  checkTokenValidity(): void {
    const token = this.getToken();
    if (token && this.isTokenExpired(token)) {
      this.logout();
      this.router.navigate(['/login']);
    }
  }

  getUserAddresses(userId: number): Observable<Address[]> {
    return this.http.get<Address[]>(`${this.apiUrl}/addresses?userId=${userId}`);
  }

  getAddressById(id: number): Observable<Address> {
    return this.http.get<Address>(`${this.apiUrl}/address/${id}`);
  }

  addAddress(address: Address): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/add-address`, address);
  }

  updateAddress(address: Address): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update-address`, address );
  }

  deleteAddress(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete-address/${id}`);
  }

  getUserById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/user-by-id/${id}`);
  }
}
