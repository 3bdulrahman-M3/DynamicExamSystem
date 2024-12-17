import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private tokenKey = 'authToken';
  private jwtHelper = new JwtHelperService();
  private apiUrl = 'http://localhost:5063/api/Authentcation/Register'; // Your API endpoint

  constructor(private http: HttpClient) {}

  // Login method
  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(
      'http://localhost:5063/api/Authentcation/Login',
      { email, password }
    );
  }

  // Save token, user role, and nameId in localStorage
  saveAuthData(token: string, role: string) {
    const decodedToken = this.jwtHelper.decodeToken(token); // Decode the token
    const nameId = decodedToken['nameid']; // Extract the nameid from the token

    // Store the token, role, and nameid in localStorage
    localStorage.setItem('token', token);
    localStorage.setItem('userRole', role);
    localStorage.setItem('nameid', nameId); // Save nameid in localStorage
  }

  // Get the JWT token from localStorage
  getToken(): string {
    return localStorage.getItem('token') || ''; // Return the token
  }

  // Get the user role from the decoded token
  getUserRole(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken['role']; // Assuming 'role' is the key in the token
    }
    return null;
  }

  // Check if the user is logged in (token is valid and not expired)
  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  // Logout the user (clear localStorage)
  logout() {
    localStorage.clear(); // Clear all localStorage data
  }

  // Register user method (if needed for the app)
  register(userData: {
    name: string;
    email: string;
    password: string;
  }): Observable<any> {
    return this.http.post(this.apiUrl, userData); // Register the user
  }

  // Get nameid from localStorage (extracted from the decoded token)
  getUserId(): string | null {
    return localStorage.getItem('nameid'); // Retrieve the nameid from localStorage
  }

  // Get username from the token (if included in the JWT payload)
  getUserName(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken['sub']; // Assuming 'sub' is the key for the username
    }
    return null;
  }
}
