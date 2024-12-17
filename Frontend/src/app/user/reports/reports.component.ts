import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
  standalone: true,
  imports: [CommonModule], // Add this to ensure `ngClass` and `date` pipe work
})
export class ReportsComponent implements OnInit {
  examResults: any[] = [];
  errorMessage: string = '';
  role: string | null = null; // User's role
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 100; 
  totalPages: number = 0;
  userId = '';
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.totalPages = Math.ceil(this.totalCount / this.pageSize);
    this.userId = this.authService.getUserId()!;
    console.log(this.userId);
    this.getExamResults();
  }

  getExamResults(): void {
    // Call your API to get exam results here
    this.http
      .get<any[]>(`http://localhost:5063/api/Exam/history/${this.userId}`)
      .subscribe({
        next: (data: any) => {
          this.examResults = data?.data;
          console.log(this.examResults);
        },
        error: (err) => {
          console.error('Failed to fetch exam results', err);
          this.errorMessage =
            'Could not fetch exam results. Please try again later.';
        },
      });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.getExamResults(); // Refresh data for the previous page
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.getExamResults(); // Refresh data for the next page
    }
  }
}
