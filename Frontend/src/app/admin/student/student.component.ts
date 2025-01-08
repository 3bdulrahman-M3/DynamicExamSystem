import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css'],
  imports: [NgFor, NgIf],
})
export class StudentComponent implements OnInit {
  students: any[] = []; 
  isLoading: boolean = false; 
  errorMessage: string = ''; 
  private apiUrl = 'http://localhost:5063/api/Account/all';
  private deleteUrl = 'http://localhost:5063/api/Account'; 

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadStudents(); 
  }

  loadStudents(): void {
    this.isLoading = true; 
    this.errorMessage = ''; 


    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.students = data; 
        this.isLoading = false; 
      },
      error: (error) => {
        this.errorMessage = 'Failed to load students'; 
        this.isLoading = false; 
      },
    });
  }

  deleteUser(id: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.isLoading = true; 
      this.http.delete(`${this.deleteUrl}/${id}`).subscribe({
        next: (data) => {
          this.students = this.students.filter((student) => student.id !== id);
          this.isLoading = false; 
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete user'; 
          this.isLoading = false;
        },
      });
    }
  }
}
