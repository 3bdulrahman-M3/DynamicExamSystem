import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../services/auth.service'; 
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  imports: [NgIf, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  role: string | null = null;

  constructor(private authService: AuthenticationService) {}
  ngOnInit(): void {
    this.role = this.authService.getUserRole();
    console.log(this.role);
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn(); 
  }

  logout(): void {
    this.authService.logout(); 
    window.location.href = '/login'; 
  }
}
