import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from '../services/auth.service'; // Update the path as needed
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [NgIf],
})
export class ProfileComponent implements OnInit {
  userId: string | null = null; // Store the user ID (nameid)
  userProfile: any = {}; // Store user profile data
  errorMessage: string = ''; // Store error message
  isLoading: boolean = true; // For loading state
  role: string = '';

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    // Retrieve the userId from localStorage (or you can get it from the token)
    this.userId = this.authService.getUserId();
    if (this.userId) {
      this.fetchUserProfile(); // Fetch the user profile using the userId
    } else {
      this.errorMessage = 'User ID not found. Please log in again.';
      this.isLoading = false;
    }
    this.role = this.authService.getUserRole()!;
  }

  // Fetch user profile using the /api/Account/{id} endpoint
  fetchUserProfile(): void {
    this.isLoading = true;
    this.http
      .get<any>(`http://localhost:5063/api/Account/${this.userId}`)
      .subscribe({
        next: (data) => {
          this.userProfile = data; // Store the fetched user profile
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
