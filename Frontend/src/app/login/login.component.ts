import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthenticationService } from '../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false; // Add a loading flag
  private authService = inject(AuthenticationService);
  private router = inject(Router);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.loading = true; // Show spinner
      const { email, password } = this.loginForm.value;

      this.authService.login(email, password).subscribe({
        next: (response: any) => {
          this.loading = false; // Hide spinner after response
          if (response && response.token) {
            const decodedToken: any = jwtDecode(response.token);

            // Save token and role
            this.authService.saveAuthData(response.token, decodedToken.role);

            // Role-based navigation
            if (decodedToken.role === 'Admin') {
              this.router.navigate(['admin/dashboard']);
            } else if (decodedToken.role === 'Student') {
              this.router.navigate(['/user/dashboard']);
            } else {
              this.errorMessage = 'Unauthorized role. Contact support.';
            }
          } else {
            this.errorMessage = 'Invalid credentials. Please try again.';
          }
        },
        error: (err) => {
          this.loading = false; // Hide spinner on error
          this.errorMessage = 'Login failed. Please try again later.';
        },
      });
    }
  }
}
