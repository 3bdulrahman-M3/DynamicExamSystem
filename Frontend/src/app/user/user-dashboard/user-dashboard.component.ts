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
  totalPassedExams: number = 0; // Number of passed exams
  totalFailedExams: number = 0; // Number of failed exams
  totalSubjects: number = 0; // Total number of subjects
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
    this.fetchTotalSubjects(); // Fetch total subjects
  }

  // Retrieve the user ID from localStorage
  getUserId(): void {
    this.nameId = this.authService.getUserId(); // Get the user ID (nameid) from localStorage
    // console.log('User ID:', this.nameId); // Example: "123"
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
          // console.log('Fetched Exam Results:', data); // Debug: check the structure of the data

          if (Array.isArray(data)) {
            this.totalExams = data.length; // Calculate the total number of exams

            // Filter the exams by final score and count passed and failed exams
            this.totalPassedExams = data.filter(
              (exam) => exam.score > 50
            ).length;
            this.totalFailedExams = data.filter(
              (exam) => exam.score <= 50
            ).length;
          } else {
            this.errorMessage = 'Received data is not in the expected format.';
          }

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

  // Fetch total subjects from the API
  fetchTotalSubjects(): void {
    this.isLoading = true;

    this.http.get<any[]>('http://localhost:5063/api/Subject').subscribe({
      next: (data) => {
        this.totalSubjects = data.length; // Calculate the total number of subjects
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to fetch subjects', err);
        this.errorMessage = 'Could not fetch subjects. Please try again later.';
        this.isLoading = false;
      },
    });
  }
}
