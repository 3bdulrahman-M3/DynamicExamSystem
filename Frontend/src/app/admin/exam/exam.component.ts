import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-exam',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, ReactiveFormsModule],
  templateUrl: './exam.component.html',
  styleUrls: ['./exam.component.css'],
})
export class ExamComponent implements OnInit {
  subjects: any[] = []; 
  exams: any[] = []; 
  examForm!: FormGroup; 
  errorMessage: string = '';
  isLoading: boolean = false;

  
  newExamTitle: string = '';
  newExamDescription: string = '';

  constructor(private http: HttpClient, private fb: FormBuilder) {
    // Initialize the form
    this.examForm = this.fb.group({
      selectedSubject: [null], 
    });
  }

  ngOnInit(): void {
    this.fetchSubjects();
  }

  fetchSubjects(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.http.get<any[]>('http://localhost:5063/api/Subject').subscribe({
      next: (data) => {
        this.subjects = data; 
        if (this.subjects.length > 0) {
        
          this.examForm.patchValue({ selectedSubject: this.subjects[0].id });
          this.fetchExams(this.subjects[0].id); 
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load subjects: ' + err.message;
        this.isLoading = false;
      },
    });
  }

  fetchExams(subjectId: number): void {
    if (!subjectId) {
      this.exams = []; 
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .get<any[]>(`http://localhost:5063/api/Exam/subject/${subjectId}`)
      .subscribe({
        next: (data) => {
          this.exams = data; 
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to load exams: ' + err.message;
          this.isLoading = false;
        },
      });
  }

  onSubjectChange(): void {
    const subjectId = this.examForm.value.selectedSubject;
    if (subjectId) {
      this.fetchExams(subjectId); 
    } else {
      this.exams = []; 
    }
  }

  // Method to add new exam
  onAddExam(): void {
    if (!this.newExamTitle.trim()) {
      this.errorMessage = 'Exam title is required.';
      return;
    }

    const newExam = {
      title: this.newExamTitle,
      description: this.newExamDescription,
      subjectId: this.examForm.value.selectedSubject,
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.http.post<any>('http://localhost:5063/api/Exam', newExam).subscribe({
      next: (response) => {
        this.exams.push(response); 
        this.newExamTitle = ''; 
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to add exam. Please try again.';
        this.isLoading = false;
      },
    });
  }

  
  onUpdateExam(examId: number, newName: string): void {
    if (!newName || newName.trim() === '') {
      this.errorMessage = 'Exam name cannot be empty.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .put(
        `http://localhost:5063/api/Exam/${examId}`,
        { newName },
        {
          headers: { 'Content-Type': 'application/json' },
          responseType: 'text', 
        }
      )
      .subscribe({
        next: () => {
          this.fetchExams(this.examForm.value.selectedSubject); 
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to update exam. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Method to delete an exam
  onDeleteExam(examId: number): void {
    if (confirm('Are you sure you want to delete this exam?')) {
      this.isLoading = true;
      this.errorMessage = '';

      this.http.delete(`http://localhost:5063/api/Exam/${examId}`).subscribe({
        next: () => {
          this.exams = this.exams.filter((exam) => exam.id !== examId); 
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Failed to delete exam. Please try again.';
          this.isLoading = false;
        },
      });
    }
  }
}
