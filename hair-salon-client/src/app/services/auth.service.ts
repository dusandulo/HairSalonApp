// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { environment } from '../environments/environment';

export interface User {
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}

interface LoginResponse {
  token: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  private apiUrl = 'http://localhost:5068/api/auth'; // Match your backend URL
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }),
    withCredentials: false
  };

  private tokenKey = 'token';

  constructor(private http: HttpClient) {
    // Initialize currentUser from localStorage if available
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      this.currentUserSubject.next(JSON.parse(savedUser));
    }
  }

  login(credentials: { email: string; password: string }): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials, this.httpOptions)
      .pipe(
        tap(response => {
          localStorage.setItem(this.tokenKey, response.token);
          const user: User = {
            email: response.email,
            firstName: response.firstName,
            lastName: response.lastName,
            role: response.role
          };
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSubject.next(user);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }
}