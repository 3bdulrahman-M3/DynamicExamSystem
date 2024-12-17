import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../../services/auth.service';

@Component({
  selector: 'app-user-dashboard',
  imports: [CommonModule],
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css'],
})
export class UserDashboardComponent implements OnInit {
  totalExams: number = 0; // Total number of exams
  errorMessage: string = ''; // For displaying errors
  isLoading: boolean = false; // For loading state
  nameId: string | null = null; // Store the user ID
  username: string = ''; // Store the user's name

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.getUserId(); // Get the user ID
    this.fetchUserDetails(); // Fetch user details using the ID
    this.calculateExamHistory(); // Calculate total exams
  }

  // Retrieve the user ID from localStorage
  getUserId(): void {
    this.nameId = this.authService.getUserId(); // Get the user ID (nameid) from localStorage
    console.log('User ID:', this.nameId); // Example: "123"
  }

  // Fetch user details using /api/Account/{id}
  fetchUserDetails(): void {
    if (!this.nameId) {
      this.errorMessage = 'User ID not found. Please log in again.';
      return;
    }

    this.http
      .get<{ userName: string; email: string }>(
        `http://localhost:5063/api/Account/${this.nameId}`
      )
      .subscribe({
        next: (data) => {
          this.username = data.userName; // Extract the username from API response
        },
        error: (err) => {
          console.error('Failed to fetch user information', err);
          this.errorMessage =
            'Could not fetch user information. Please try again later.';
        },
      });
  }

  // Fetch the total number of exams
  calculateExamHistory(): void {
    this.isLoading = true;

    this.http
      .get<any[]>('http://localhost:5063/api/Exam/exam/results')
      .subscribe({
        next: (data) => {
          this.totalExams = data.length; // Calculate the total number of exams
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch exam results', err);
          this.errorMessage =
            'Could not fetch exam history. Please try again later.';
          this.isLoading = false;
        },
      });
  }
}
