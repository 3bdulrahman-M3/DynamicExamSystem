import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/auth.service'; // Update the path as needed
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule],
})
export class SidebarAdminComponent implements OnInit {
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
