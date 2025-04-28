import { Injectable } from '@angular/core';
import { User } from '../models/user.model';
import { of, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser: User | null = null;
  private users: User[] = [
    { id: 1, email: 'user@example.com', password: 'password', fullName: 'Test User', phoneNumber: '123 Main St', isAdmin:false }
  ];

  constructor() { }

  login(email: string, password: string): Observable<User | null> {
    const user = this.users.find(u => u.email === email && u.password === password);
    this.currentUser = user || null;
    return of(this.currentUser);
  }

  register(user: User): Observable<User> {
    user.id = this.users.length + 1;
    this.users.push(user);
    this.currentUser = user;
    return of(user);
  }

  logout(): void {
    this.currentUser = null;
  }

  isAuthenticated(): boolean {
    return !!this.currentUser;
  }

  getCurrentUser(): User | null {
    return this.currentUser;
  }
}