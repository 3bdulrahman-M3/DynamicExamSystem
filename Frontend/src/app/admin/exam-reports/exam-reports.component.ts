import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgClass, NgFor, NgIf } from '@angular/common';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-exam-reports',
  templateUrl: './exam-reports.component.html',
  styleUrls: ['./exam-reports.component.css'],
  imports: [NgIf, NgFor, DatePipe, NgClass],
})
export class ExamReportsComponent implements OnInit {
  examHistory: any[] = []; // Using any[] for the exam history data
  isLoading: boolean = false;
  errorMessage: string = '';
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;
  private apiUrl = 'http://localhost:5063/api/Exam/history'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadExamHistory();
  }

  loadExamHistory(): void {
    this.isLoading = true;
    this.errorMessage = '';

    // Making the HTTP GET request directly in the component with pagination parameters
    const params = {
      pageNumber: this.pageNumber.toString(),
      pageSize: this.pageSize.toString(),
    };

    this.http.get<any>(this.apiUrl, { params }).subscribe({
      next: (data) => {
        this.examHistory = data.data || []; // Assuming `data.data` contains the exam history
        this.totalCount = data.totalCount || 0; // Assuming `data.totalCount` is the total count of exams
        this.totalPages = data.totalPages || 0; // Assuming `data.totalPages` contains the total number of pages
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load exam history';
        this.isLoading = false;
      },
    });
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadExamHistory(); // Refresh data for the previous page
    }
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadExamHistory(); // Refresh data for the next page
    }
  }
}
