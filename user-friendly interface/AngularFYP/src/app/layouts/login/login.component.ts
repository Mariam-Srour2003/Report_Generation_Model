import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

import { HttpClientModule } from '@angular/common/http';
import { environment } from '../../../environments/environment.prod';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    CommonModule,
    HttpClientModule,
    MatSnackBarModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  private apiUrl = `${environment.apiUrl}/User/Login`;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this.login(email, password).subscribe(
        response => {
          console.log('Login successful', response);
          if (response.message === null) {
            console.log(response.data)
            localStorage.setItem('userid', response.data.id);
            localStorage.setItem('role', response.data.roleId);

            this.router.navigate(['/main']);
          } else {
            // Show message for the user as toast
            this.snackBar.open(response.message, 'Close', {
              duration: 3000,
            });
          }
        },
        error => {
          console.error('Login failed', error);
          alert('Login failed: ' + this.getErrorMessage(error));
        }
      );
    }
  }

  login(username: string, password: string): Observable<any> {
    const loginData = { username, password };
    return this.http.post<any>(this.apiUrl, loginData).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    return throwError('Something bad happened; please try again later.');
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.error instanceof ErrorEvent) {
      return `An error occurred: ${error.error.message}`;
    } else {
      return `Backend returned code ${error.status}, ` +
             `body was: ${error.error}`;
    }
  }
}
