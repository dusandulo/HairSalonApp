import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Profile } from '../interfaces/profile.interface';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  isEditing = false;
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.loadUserProfile();
  }

  private initializeForm() {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      preferences: this.fb.group({
        appointmentReminders: [true],
        marketingEmails: [false]
      })
    });
  }

  private loadUserProfile() {
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.profileForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email
        });
      }
    });
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.authService.updateProfile(this.profileForm.value).subscribe({
        next: () => {
          this.successMessage = 'Profile updated successfully';
          this.isEditing = false;
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: () => {
          this.successMessage = 'Error updating profile';
        }
      });
    }
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
  }
}
