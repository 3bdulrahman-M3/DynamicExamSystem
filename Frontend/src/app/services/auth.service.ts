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
  private apiUrl = 'http://localhost:5063/api/Authentcation/Register'; 

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
    const decodedToken = this.jwtHelper.decodeToken(token); 
    const nameId = decodedToken['nameid']; 

    localStorage.setItem('token', token);
    localStorage.setItem('userRole', role);
    localStorage.setItem('nameid', nameId);
  }

  getToken(): string {
    return localStorage.getItem('token') || ''; 
  }

  // Get the user role from the decoded token
  getUserRole(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken['role']; 
    }
    return null;
  }


  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }


  logout() {
    localStorage.clear(); 
  }


  register(userData: {
    name: string;
    email: string;
    password: string;
  }): Observable<any> {
    return this.http.post(this.apiUrl, userData); 
  }

  getUserId(): string | null {
    return localStorage.getItem('nameid'); 
  }

  getUserName(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken['sub']; 
    }
    return null;
  }
}
