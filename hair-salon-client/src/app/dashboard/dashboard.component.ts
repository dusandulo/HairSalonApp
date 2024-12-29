import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AppointmentService } from '../services/appointment.service';
import { AuthService, User } from '../services/auth.service';
import { Observable } from 'rxjs';
import { Appointment } from '../interfaces/appointment.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, DatePipe]
})
export class DashboardComponent implements OnInit {
  availableAppointments: Appointment[] = [];
  userAppointments: Appointment[] = [];
  user$!: Observable<User | null>;
  activeTab: string = 'dashboard';
  isMenuOpen: boolean = false;

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.user$ = this.authService.currentUser$;
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentService.getAvailableAppointments()
      .subscribe(appointments => this.availableAppointments = appointments);
    
    this.appointmentService.getUserAppointments()
      .subscribe(appointments => this.userAppointments = appointments);
  }

  bookAppointment(appointmentId: number) {
    this.appointmentService.bookAppointment(appointmentId)
      .subscribe(() => this.loadAppointments());
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  navigateToProfile() {
    this.setActiveTab('profile');
    this.router.navigate(['/profile']);
  }
}
