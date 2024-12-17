import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/auth.service';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-subject',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './subject.component.html',
  styleUrls: ['./subject.component.css'],
})
export class SubjectComponent implements OnInit {
  subjects: any[] = [];
  subjectForm: FormGroup;
  isEditing: boolean = false;
  editingSubjectId: number | null = null;
  errorMessage: string = '';
  headers: HttpHeaders = new HttpHeaders();

  private apiBase = 'http://localhost:5063/api/Subject';

  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private authService: AuthenticationService
  ) {
    this.subjectForm = this.fb.group({
      name: ['', Validators.required],
    });
    this.headers = this.headers.append('Content-Type', 'application/json');
    this.headers = this.headers.append('Accept', 'application/json');
    this.headers = this.headers.append(
      'Authorization',
      `Bearer ${this.authService.getToken()}`
    );
  }

  ngOnInit(): void {
    this.getAllSubjects();
  }

  // Fetch all subjects
  getAllSubjects(): void {
    this.http
      .get<any[]>(`${this.apiBase}`, { headers: this.headers })
      .subscribe({
        next: (data) => (this.subjects = data),
        error: (err) =>
          (this.errorMessage = 'Failed to load subjects.' + err.message),
      });
  }

  // Add a new subject
  addSubject(): void {
    if (this.subjectForm.valid) {
      // Create a new FormData object
      const formData = new FormData();

      // Append form values to the FormData object
      formData.append('Name', this.subjectForm.value.name); // Assuming 'name' is the field name in the form

      // Now, send the FormData via HTTP POST
      this.http.post(`${this.apiBase}/Addsubject`, formData).subscribe({
        next: () => {
          this.subjectForm.reset();
          this.getAllSubjects();
        },
        error: () => (this.errorMessage = 'Failed to add subject.'),
      });
    }
  }

  // Start editing a subject
  startEdit(subject: any): void {
    this.isEditing = true;
    this.editingSubjectId = subject.id;
    this.subjectForm.patchValue({ name: subject.name });
  }

  // Save edited subject
  saveEdit(): void {
    if (this.subjectForm.valid && this.editingSubjectId !== null) {
      // Create FormData for the PUT request
      const formData = new FormData();
      formData.append('Name', this.subjectForm.value.name); // Make sure 'name' is correct

      this.http
        .put(`${this.apiBase}/${this.editingSubjectId}`, formData)
        .subscribe({
          next: () => {
            this.isEditing = false;
            this.editingSubjectId = null;
            this.subjectForm.reset();
            this.getAllSubjects();
          },
          error: (err) => {
            console.error(err);
            this.errorMessage = 'Failed to edit subject.';
          },
        });
    }
  }

  // Delete a subject
  deleteSubject(id: number): void {
    if (confirm('Are you sure you want to delete this subject?')) {
      this.http.delete(`${this.apiBase}/${id}`).subscribe({
        next: () => this.getAllSubjects(),
        error: () => (this.errorMessage = 'Failed to delete subject.'),
      });
    }
  }
}
