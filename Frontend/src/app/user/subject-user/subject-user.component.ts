import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-subject-user',
  templateUrl: './subject-user.component.html',
  styleUrls: ['./subject-user.component.css'],
  standalone: true,
  imports: [CommonModule],
})
export class SubjectUserComponent implements OnInit {
  subjects: any[] = [];
  exams: any[] = [];
  private baseApiUrl = 'http://localhost:5063/api';
  Role = localStorage.getItem('userRole');

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.http.get<any[]>(`${this.baseApiUrl}/Subject`).subscribe({
      next: (data) => (this.subjects = data),
      error: (err) => console.error('Failed to load subjects', err),
    });
  }

  onSubjectChange(event: Event): void {
    const target = event.target as HTMLSelectElement; 
    const subjectId = target.value; 
    if (!subjectId) {
      this.exams = [];
      return;
    }
    this.fetchExamsBySubject(subjectId); 
  }

  fetchExamsBySubject(subjectId: string): void {
    this.http
      .get<any[]>(`${this.baseApiUrl}/Exam/subject/${subjectId}`)
      .subscribe({
        next: (data) => (this.exams = data),
        error: (err) => {
          console.error('Failed to load exams', err);
          this.exams = [];
        },
      });
  }

  startExam(examId: number): void {
    console.log('Navigating to exam:', examId);
    this.router.navigate(['user/start-exam', examId]);
  }

  startRandomExam(): void {
    if (this.exams.length > 0) {
      const randomIndex = Math.floor(Math.random() * this.exams.length);
      const randomExam = this.exams[randomIndex];
      console.log('Navigating to exam:', randomExam.id);

      // Pass exam ID and title as parameters
      this.router.navigate(['user/start-exam', randomExam.id], {
        queryParams: { title: randomExam.title },
      });
    } else {
      console.log('No exams available to start');
    }
  }
}
