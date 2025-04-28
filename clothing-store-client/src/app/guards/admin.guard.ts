import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanLoad } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate, CanLoad {
  constructor(private accountService: AccountService, private router: Router) {}

  private checkAdmin(): Observable<boolean> {
    return this.accountService.isAdmin$.pipe(
      take(1),
      map(isAdmin => {
        if (!isAdmin) {
          this.router.navigate(['/']);
        }
        return isAdmin;
      })
    );
  }

  canActivate(): Observable<boolean> {
    return this.checkAdmin();
  }

  canLoad(): Observable<boolean> {
    return this.checkAdmin();
  }
}