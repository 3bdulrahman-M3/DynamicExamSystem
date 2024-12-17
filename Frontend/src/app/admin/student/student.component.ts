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
  students: any[] = []; // Store the students data
  isLoading: boolean = false; // Flag to show loading indicator
  errorMessage: string = ''; // Error message if fetching fails
  private apiUrl = 'http://localhost:5063/api/Account/all'; // API URL to fetch students
  private deleteUrl = 'http://localhost:5063/api/Account'; // API URL to delete user

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadStudents(); // Load students when the component is initialized
  }

  loadStudents(): void {
    this.isLoading = true; // Set loading flag to true
    this.errorMessage = ''; // Reset error message

    // Make the HTTP GET request to the API
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.students = data; // Store the fetched students in the component
        this.isLoading = false; // Set loading flag to false when data is fetched
      },
      error: (error) => {
        this.errorMessage = 'Failed to load students'; // Set error message if the request fails
        this.isLoading = false; // Set loading flag to false
      },
    });
  }

  deleteUser(id: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.isLoading = true; // Set loading flag to true

      // Make the HTTP DELETE request to the API to delete the user by id
      this.http.delete(`${this.deleteUrl}/${id}`).subscribe({
        next: (data) => {
          // Remove the user from the students array if deleted successfully
          this.students = this.students.filter((student) => student.id !== id);
          this.isLoading = false; // Set loading flag to false
        },
        error: (error) => {
          this.errorMessage = 'Failed to delete user'; // Set error message if the delete fails
          this.isLoading = false; // Set loading flag to false
        },
      });
    }
  }
}
