import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthenticationService } from './../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class ReportsComponent implements OnInit {
  examResults: any[] = [];
  errorMessage: string = '';
  role: string | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;
  userId: string = '';
  isLoading: boolean = false;

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId()!;
    this.getExamResults();
  }

  /**
   * Fetch exam results with pagination.
   */
  getExamResults(): void {
    this.isLoading = true; // Start loading spinner

    const params = new HttpParams()
      .set('pageNumber', this.pageNumber.toString())
      .set('pageSize', this.pageSize.toString());

    this.http
      .get<any>(`http://localhost:5063/api/Exam/history/${this.userId}`, {
        params,
      })
      .subscribe({
        next: (data) => {
          this.examResults = data?.data || [];
          this.totalCount = data?.totalCount || 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.errorMessage = ''; // Clear any previous errors
          this.isLoading = false; // Stop loading spinner
        },
        error: (err) => {
          console.error('Failed to fetch exam results:', err);
          this.errorMessage =
            'Could not fetch exam results. Please try again later.';
          this.isLoading = false; // Stop loading spinner even on error
        },
      });
  }

  /**
   * Navigate to the previous page.
   */
  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.getExamResults();
    }
  }

  /**
   * Navigate to the next page.
   */
  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.getExamResults();
    }
  }

  /**
   * Reset pagination and reload data.
   */
  resetPagination(): void {
    this.pageNumber = 1;
    this.getExamResults();
  }

  /**
   * Handle changes to the page size and reset pagination.
   */
  onPageSizeChange(): void {
    this.resetPagination();
  }
}
