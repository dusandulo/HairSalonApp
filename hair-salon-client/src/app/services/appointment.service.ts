import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Appointment } from '../interfaces/appointment.interface';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AppointmentService {
  private apiUrl = 'http://localhost:5068/api/appointments';

  constructor(private http: HttpClient) {}

  getAvailableAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(this.apiUrl).pipe(
      catchError((error) => {
        console.error('Error fetching appointments:', error);
        return throwError(() => error);
      })
    );
  }

  getUserAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${this.apiUrl}/history`).pipe(
      catchError((error) => {
        console.error('Error fetching user appointments:', error);
        return throwError(() => error);
      })
    );
  }

  bookAppointment(id: number): Observable<Appointment> {
    return this.http.post<Appointment>(`${this.apiUrl}/book/${id}`, {}).pipe(
      catchError((error) => {
        console.error('Error booking appointment:', error);
        return throwError(() => error);
      })
    );
  }
}
