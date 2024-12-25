import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { animate, style, transition, trigger, keyframes } from '@angular/animations';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('0.3s ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ]),
    trigger('shake', [
      transition('* => *', [
        animate('0.5s', keyframes([
          style({ transform: 'translateX(0)', offset: 0 }),
          style({ transform: 'translateX(-10px)', offset: 0.25 }),
          style({ transform: 'translateX(10px)', offset: 0.75 }),
          style({ transform: 'translateX(0)', offset: 1 })
        ]))
      ])
    ])
  ]
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    // Add this for debugging
    this.loginForm.statusChanges.subscribe(status => {
      console.log('Form Status:', status);
      console.log('Form Errors:', this.loginForm.errors);
      console.log('Email Errors:', this.loginForm.get('email')?.errors);
      console.log('Password Errors:', this.loginForm.get('password')?.errors);
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.loading = true;
      this.errorMessage = '';

      const credentials = {
        email: this.loginForm.get('email')?.value,
        password: this.loginForm.get('password')?.value
      };

      this.authService.login(credentials).subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('Login error:', error);
          if (error.status === 401) {
            this.errorMessage = 'Invalid email or password';
          } else {
            this.errorMessage = 'Login failed. Please try again.';
          }
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}
