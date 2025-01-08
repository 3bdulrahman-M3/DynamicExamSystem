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
  totalExams: number = 0; 
  totalPassedExams: number = 0; 
  totalFailedExams: number = 0; 
  totalSubjects: number = 0; 
  errorMessage: string = ''; 
  isLoading: boolean = false;
  nameId: string | null = null; 
  username: string = ''; 

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.getUserId(); 
    this.fetchUserDetails(); 
    this.calculateExamHistory();
    this.fetchTotalSubjects(); 
  }

  getUserId(): void {
    this.nameId = this.authService.getUserId(); 

  }
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
          this.username = data.userName; 
        },
        error: (err) => {
          console.error('Failed to fetch user information', err);
          this.errorMessage =
            'Could not fetch user information. Please try again later.';
        },
      });
  }

  calculateExamHistory(): void {
    this.isLoading = true;

    this.http
      .get<any[]>('http://localhost:5063/api/Exam/exam/results')
      .subscribe({
        next: (data) => {
          

          if (Array.isArray(data)) {
            this.totalExams = data.length; 

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
  fetchTotalSubjects(): void {
    this.isLoading = true;

    this.http.get<any[]>('http://localhost:5063/api/Subject').subscribe({
      next: (data) => {
        this.totalSubjects = data.length; 
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
