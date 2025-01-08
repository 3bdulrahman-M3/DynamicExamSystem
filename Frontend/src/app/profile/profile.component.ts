import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from '../services/auth.service'; 
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [NgIf],
})
export class ProfileComponent implements OnInit {
  userId: string | null = null; 
  userProfile: any = {}; 
  errorMessage: string = ''; 
  isLoading: boolean = true;
  role: string = '';

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {

    this.userId = this.authService.getUserId();
    if (this.userId) {
      this.fetchUserProfile(); 
    } else {
      this.errorMessage = 'User ID not found. Please log in again.';
      this.isLoading = false;
    }
    this.role = this.authService.getUserRole()!;
  }

  fetchUserProfile(): void {
    this.isLoading = true;
    this.http
      .get<any>(`http://localhost:5063/api/Account/${this.userId}`)
      .subscribe({
        next: (data) => {
          this.userProfile = data; 
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch user profile', err);
          this.errorMessage =
            'Could not fetch user profile. Please try again later.';
          this.isLoading = false;
        },
      });
  }
}
