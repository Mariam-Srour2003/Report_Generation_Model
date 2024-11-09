import { Component, OnInit, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
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
  selector: 'app-update-appointment',
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
    templateUrl: './update-appointment.component.html',
  styleUrl: './update-appointment.component.scss'
})
export class UpdateAppointmentComponent implements OnInit {
  UserForm: FormGroup;
  doctors: any[] = [];  // To store users with roleId = 2
  appointmentId: number;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<UpdateAppointmentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.appointmentId = data.Id;
    console.log("ggg")
    console.log(data.Id)
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
    this.initializeForm();
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

  initializeForm(): void {
  this.http.get<{ statusCode: number, message: string, data: any }>(`https://localhost:7106/api/Appointment/GetAppointmentById/${this.appointmentId}`)
    .subscribe(
      response => {
        if (response.statusCode === 200) {
          const appointment = response.data;
          console.log('Fetched appointment data:', appointment);

          // Ensure the date and time are correctly extracted and formatted
          const appointmentDate = new Date(appointment.appointmentDate);
          const appointmentTime = this.formatTime(appointmentDate);

          console.log('Parsed Date:', appointmentDate);
          console.log('Parsed Time:', appointmentTime);

          // Populate the form with fetched data
          this.UserForm.patchValue({
            patientId: appointment.patientId,
            doctorId: appointment.doctorId,
            appointmentDate: appointmentDate,
            appointmentTime: appointmentTime,
            status: appointment.status
          });

          console.log('Form Values after patchValue:', this.UserForm.value);
        } else {
          this.snackBar.open(`Failed to load appointment: ${response.message}`, 'Close', { duration: 3000 });
        }
      },
      error => {
        console.error('Failed to load appointment', error);
        this.snackBar.open('Failed to load appointment', 'Close', { duration: 3000 });
      }
    );
}


  formatTime(date: Date): string {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  onSubmit(): void {
    if (this.UserForm.valid) {
      const appointmentData = this.combineDateAndTime();

      this.http.put<{ statusCode: number, message: string }>(`https://localhost:7106/api/Appointment/UpdateAppointment`, {
        ...appointmentData,
        id: this.appointmentId
      })
        .subscribe(
          response => {
            if (response.statusCode === 200) {
              this.snackBar.open('Appointment updated successfully!', 'Close', { duration: 3000 });
              this.dialogRef.close(appointmentData);
            } else {
              this.snackBar.open(`Failed to update appointment: ${response.message}`, 'Close', { duration: 3000 });
            }
          },
          error => {
            console.error('Failed to update appointment', error);
            this.snackBar.open('Failed to update appointment', 'Close', { duration: 3000 });
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
