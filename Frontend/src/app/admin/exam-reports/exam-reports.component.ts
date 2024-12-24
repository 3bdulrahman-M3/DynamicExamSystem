import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-exam-reports',
  templateUrl: './exam-reports.component.html',
  styleUrls: ['./exam-reports.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule], // Ensures ngClass and date pipe work
})
export class ExamReportsComponent implements OnInit {
  examHistory: any[] = [];
  errorMessage: string = '';
  pageNumber: number = 1;
  pageSize: number = 10; // Default page size
  totalCount: number = 0;
  totalPages: number = 0;
  isLoading: boolean = false; // Flag to show loading indicator

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getExamHistory(); // Fetch exam history when the component is initialized

  }

  getExamHistory(): void {
    this.isLoading = true; // Show loading indicator
    const params = new HttpParams()
      .set('pageNumber', this.pageNumber.toString())
      .set('pageSize', this.pageSize.toString());

    // Fetch exam history for all users
    this.http
      .get<any>(`http://localhost:5063/api/Exam/history`, { params })
      .subscribe({
        next: (data) => {
          this.examHistory = data?.data;
          this.totalCount = data?.totalCount;
          this.totalPages = data?.totalPages
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Failed to fetch exam results', err);
          this.errorMessage =
            'Could not fetch exam results. Please try again later.';
          this.isLoading = false;
        },
      });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.getExamHistory();
    }
  }

  nextPage(): void {
    if (this.examHistory != null) {
      this.pageNumber++;
      this.getExamHistory();
    }

  }

  onPageSizeChange(): void {
    this.pageNumber = 1;
    this.getExamHistory();
  }
}
