import {AfterViewInit, Component, ViewChild, OnInit} from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatSort, MatSortModule} from '@angular/material/sort';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialog
} from '@angular/material/dialog';
import { CreateAppointmentComponent } from './create-appointment/create-appointment.component'; 
import { UpdateAppointmentComponent } from './update-appointment/update-appointment.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
export interface AppointmentData {
  id: number;
  patientId: number;
  appointmentDate: Date;
  doctorId: number;
  status: number;
}
@Component({
  selector: 'app-all-appointments',
  standalone: true,
  imports: [
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    CommonModule,
    MatSortModule,
    MatPaginatorModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './all-appointments.component.html',
  styleUrl: './all-appointments.component.scss'
})
export class AllAppointmentsComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [ 'patientId', 'doctorId', 'appointmentDate', 'actions'];
  dataSource: MatTableDataSource<AppointmentData>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  ispatient!: boolean
  constructor(private http: HttpClient, public dialog: MatDialog, private snackBar: MatSnackBar,) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit() {
    console.log("hello")
    this.fetchAppointments();
    console.log(localStorage.getItem('role'))
    this.ispatient =(localStorage.getItem('role') === '3')
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  fetchAppointments() {
    if (localStorage.getItem('role')==='1') {
      this.http.get<{ statusCode: number, message: string, data: AppointmentData[] }>('https://localhost:7106/api/Appointment/GetAllAppointments')
      .subscribe(response => {
        console.log(response)
        if (response.data === null) {
          console.error('Failed to fetch appointments', response.message);
        } else {
          this.dataSource.data = response.data;
        }
      }, error => {
        console.error('Failed to fetch appointments', error);
      });
    } else if (localStorage.getItem('role') === '2') {
      const appointmentid =localStorage.getItem('userid')
      this.http.get<{ statusCode: number, message: string, data: AppointmentData[] }>('https://localhost:7106/api/Appointment/GetAppointmentsByDoctorId/'+ appointmentid)
      .subscribe(response => {
        console.log(response)
        if (response.data === null) {
          console.error('Failed to fetch appointments', response.message);
        } else {
          this.dataSource.data = response.data;
        }
      }, error => {
        console.error('Failed to fetch appointments', error);
      });
    }else if (localStorage.getItem('role')=== '3') {
      const appointmentid =localStorage.getItem('userid')
      this.http.get<{ statusCode: number, message: string, data: AppointmentData[] }>('https://localhost:7106/api/Appointment/GetAppointmentsByPatientId/'+ appointmentid)
      .subscribe(response => {
        console.log(response)
        if (response.data === null) {
          console.error('Failed to fetch appointments', response.message);
        } else {
          this.dataSource.data = response.data;
        }
      }, error => {
        console.error('Failed to fetch appointments', error);
      });
    }
    
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(CreateAppointmentComponent, {
      width: '800px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    // Refresh data after the dialog is closed
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchAppointments();
      }
    });
  }

openUpdateDialog(enterAnimationDuration: string, exitAnimationDuration: string, ID: number): void {
    const dialogRef = this.dialog.open(UpdateAppointmentComponent, {
      width: '800px',
      enterAnimationDuration,
      exitAnimationDuration,
      data: { Id: ID }
    });

    // Refresh data after the dialog is closed
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchAppointments();
      }
    });
  }

  DeleteAppointment(appointmentId: number): void {
    this.http.delete<{ statusCode: number, message: string }>(`https://localhost:7106/api/Appointment/Delete/${appointmentId}`)
      .subscribe(
        response => {
          if (response.statusCode === 200) {
            this.snackBar.open('appointment deleted successfully!', 'Close', { duration: 3000 });
            this.fetchAppointments(); // Refresh the appointment list after deletion
          } else {
            this.snackBar.open(`Failed to delete appointment: ${response.message}`, 'Close', { duration: 3000 });
          }
        },
        error => {
          console.error('Failed to delete appointment', error);
          this.snackBar.open('Failed to delete appointment', 'Close', { duration: 3000 });
        }
      );
  }
  AcceptAppointment(appointmentId: number): void {
  // Step 1: Fetch the current appointment details
  this.http.get<{ statusCode: number, message: string, data: any }>(`https://localhost:7106/api/Appointment/GetAppointmentById/${appointmentId}`)
    .subscribe(
      response => {
        if (response.statusCode === 200) {
          const appointment = response.data;

          // Step 2: Update the status to 1
          const updatedAppointment = {
            ...appointment,
            status: 1  // Change the status to 1
          };

          // Step 3: Send the updated appointment data to the UpdateAppointment API
          this.http.put<{ statusCode: number, message: string }>(`https://localhost:7106/api/Appointment/UpdateAppointment`, updatedAppointment)
            .subscribe(
              updateResponse => {
                if (updateResponse.statusCode === 200) {
                  this.snackBar.open('Appointment status updated successfully!', 'Close', { duration: 3000 });
                } else {
                  this.snackBar.open(`Failed to update appointment status: ${updateResponse.message}`, 'Close', { duration: 3000 });
                }
              },
              error => {
                console.error('Failed to update appointment status', error);
                this.snackBar.open('Failed to update appointment status', 'Close', { duration: 3000 });
              }
            );
        } else {
          this.snackBar.open(`Failed to fetch appointment: ${response.message}`, 'Close', { duration: 3000 });
        }
      },
      error => {
        console.error('Failed to fetch appointment', error);
        this.snackBar.open('Failed to fetch appointment', 'Close', { duration: 3000 });
      }
    );
}

}
