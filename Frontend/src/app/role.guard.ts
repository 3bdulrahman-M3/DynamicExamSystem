import { Injectable, inject } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  private router = inject(Router);

  canActivate(): boolean {
    const userRole = localStorage.getItem('userRole');

    if (!userRole) {
      this.router.navigate(['/login']);
      return false; // Redirect to login if no role is found
    }

    return true;
  }
}
