import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { animate, style, transition, trigger, keyframes } from '@angular/animations';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
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
export class RegisterComponent {
  isLoading = false;
  errorMessage = '';

  registerForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required)
  });

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      
      this.http.post('http://localhost:5068/api/Auth/register', this.registerForm.value)
        .subscribe({
          next: (response) => {
            this.router.navigate(['/login']);
          },
          error: (error) => {
            this.errorMessage = 'Registration failed. Please try again.';
            this.isLoading = false;
          },
          complete: () => {
            this.isLoading = false;
          }
        });
    }
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
