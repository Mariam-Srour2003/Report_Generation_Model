import { Component, OnInit } from '@angular/core';
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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-create-appointment',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
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
  templateUrl: './create-appointment.component.html',
  styleUrls: ['./create-appointment.component.scss']
})
export class CreateAppointmentComponent implements OnInit {
  UserForm: FormGroup;
  doctors: any[] = [];  // To store users with roleId = 2

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<CreateAppointmentComponent>
  ) {
    this.UserForm = this.fb.group({
      patientId: [localStorage.getItem('userid'), Validators.required],
      doctorId: ['', Validators.required],
      appointmentDate: ['', Validators.required],
      appointmentTime: ['', Validators.required],
      status: [0, Validators.required]
    });
  }

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers(): void {
    this.http.get<{ statusCode: number, message: string, data: any[] }>('https://localhost:7106/api/User/GetAllUsers')
      .subscribe(
        response => {
          if (response.statusCode === 200) {
            // Filter users with roleId = 2
            this.doctors = response.data.filter(user => user.roleId === 2);
          } else {
            this.snackBar.open(`Failed to fetch users: ${response.message}`, 'Close', { duration: 3000 });
          }
        },
        error => {
          console.error('Failed to fetch users', error);
          this.snackBar.open('Failed to fetch users', 'Close', { duration: 3000 });
        }
      );
  }
  
  onSubmit(): void {
    if (this.UserForm.valid) {
      const appointmentData = this.combineDateAndTime();

      this.http.post<{ statusCode: number, message: string }>('https://localhost:7106/api/Appointment/AddAppointment', appointmentData)
        .subscribe(
          response => {
            if (response.statusCode === 200) {
              this.snackBar.open('Appointment added successfully!', 'Close', { duration: 3000 });
              this.dialogRef.close(appointmentData);
            } else {
              this.snackBar.open(`Failed to add appointment: ${response.message}`, 'Close', { duration: 3000 });
            }
          },
          error => {
            console.error('Failed to register user', error);
            this.snackBar.open('Failed to register user', 'Close', { duration: 3000 });
          }
        );
    }
  }

  combineDateAndTime(): any {
    const date = this.UserForm.value.appointmentDate;
    const time = this.UserForm.value.appointmentTime;
    
    const [hours, minutes] = time.split(':');
    const combinedDateTime = new Date(date);
    combinedDateTime.setHours(+hours);
    combinedDateTime.setMinutes(+minutes);

    return {
      ...this.UserForm.value,
      appointmentDate: combinedDateTime.toISOString(), 
    };
  }
}
