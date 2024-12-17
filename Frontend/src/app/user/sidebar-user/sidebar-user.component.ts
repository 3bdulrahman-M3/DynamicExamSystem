import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar-user',
  imports: [CommonModule],
  templateUrl: './sidebar-user.component.html',
  styleUrl: './sidebar-user.component.css',
})
export class SidebarUserComponent {
  role: string | null = null;

  constructor(private authService: AuthenticationService) {}

  ngOnInit(): void {
    this.role = this.authService.getUserRole();
    console.log(); // Fetch user role on initialization
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn(); // Check login state
  }

  logout(): void {
    this.authService.logout(); // Clear auth data
    window.location.href = '/login'; // Redirect to login page
  }
}
