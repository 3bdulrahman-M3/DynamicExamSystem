import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SubjectService {
  private apiBase = 'http://localhost:5063/api';

  constructor(private http: HttpClient) {}

  getAllSubjects(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/Subject/AllSubjects`);
  }

  getExamsBySubject(subjectId: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiBase}/Exam/ExamsBySubject/${subjectId}`
    );
  }
}
