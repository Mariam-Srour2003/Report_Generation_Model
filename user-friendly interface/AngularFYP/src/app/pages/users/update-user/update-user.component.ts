import { Component, Inject, OnInit } from '@angular/core';
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
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-update-user',
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
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.scss'
})
export class UpdateUserComponent implements OnInit {
  UserForm: FormGroup;
  userId: number;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<UpdateUserComponent>
  ) {
    this.userId = data.userId;
    this.UserForm = this.fb.group({
      id: this.userId,
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
  ngOnInit(): void {
    this.fetchUserData();
  }
  fetchUserData(): void {
    this.http.get<{ statusCode: number, message: string, data: any }>(`https://localhost:7106/api/User/GetUserById/${this.userId}`)
      .subscribe(
        response => {
          if (response.statusCode === 200 && response.data) {
            // Populate the form with fetched data
            this.UserForm.patchValue({
              firstName: response.data.firstName,
              lastName: response.data.lastName,
              email: response.data.email,
              roleId: response.data.roleId
            });
          } else {
            console.error('Failed to fetch user data', response.message);
          }
        },
        error => {
          console.error('Failed to fetch user data', error);
        }
      );
  }
  onSubmit(): void {
    if (this.UserForm.valid) {
      const userData = this.UserForm.value;

      // Make a PUT request to update the user information
      this.http.put<{ statusCode: number, message: string }>(`https://localhost:7106/api/User/Update/${this.userId}`, userData)
        .subscribe(
          response => {
            if (response.statusCode === 200) {
              this.snackBar.open('User updated successfully!', 'Close', { duration: 3000 });
              this.dialogRef.close(userData);
            } else {
              this.snackBar.open(`Failed to update user: ${response.message}`, 'Close', { duration: 3000 });
            }
          },
          error => {
            console.error('Failed to update user', error);
            this.snackBar.open('Failed to update user', 'Close', { duration: 3000 });
          }
        );
    }
  }

}
