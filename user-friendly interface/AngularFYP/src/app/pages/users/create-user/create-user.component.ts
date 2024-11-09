import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
} from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDialogClose,
    MatDialogActions,
    MatDialogTitle,
    MatDialogContent,
    HttpClientModule,
    MatDatepickerModule
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.scss'
})
export class CreateUserComponent {
  UserForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<CreateUserComponent>
  ) {
    this.UserForm = this.fb.group({
      username: ['', Validators.required],
      firstName: ['', Validators.required],
      middleName: ['', Validators.required],
      lastName: ['', Validators.required],
      gender: ['', Validators.required],
      roleId: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['P@$$w0rd'],
      dateOfBirth: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    console.log("out")
    if (this.UserForm.valid) {
      console.log("in")
      const userData = this.UserForm.value;

      this.http.post<{ statusCode: number, message: string }>('https://localhost:7106/api/User/register', userData)
        .subscribe(
          response => {
            if (response.statusCode === 200) {
              this.snackBar.open('User registered successfully!', 'Close', { duration: 3000 });
              this.dialogRef.close(userData);
            } else {
              this.snackBar.open(`Failed to register user: ${response.message}`, 'Close', { duration: 3000 });
            }
          },
          error => {
            console.error('Failed to register user', error);
            this.snackBar.open('Failed to register user', 'Close', { duration: 3000 });
          }
        );
    }
  }
}
