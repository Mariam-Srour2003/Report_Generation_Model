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
import { CreateUserComponent } from './create-user/create-user.component';
import { UpdateUserComponent } from './update-user/update-user.component';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface UserData {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  roleId: number;
}

@Component({
  selector: 'app-users',
  styleUrl: './users.component.scss',
  templateUrl: './users.component.html',
  standalone: true,
  imports: [
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
})
export class UsersComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = [ 'firstName', 'lastName', 'email', 'actions'];
  dataSource: MatTableDataSource<UserData>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private http: HttpClient, public dialog: MatDialog, private snackBar: MatSnackBar,) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit() {
    console.log("hello")
    this.fetchUsers();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  fetchUsers() {
    this.http.get<{ statusCode: number, message: string, data: UserData[] }>('https://localhost:7106/api/User/GetAllUsers')
      .subscribe(response => {
        console.log(response)
        if (response.data === null) {
          console.error('Failed to fetch users', response.message);
        } else {
          this.dataSource.data = response.data;
        }
      }, error => {
        console.error('Failed to fetch users', error);
      });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  openDialog(enterAnimationDuration: string, exitAnimationDuration: string): void {
    const dialogRef = this.dialog.open(CreateUserComponent, {
      width: '800px',
      enterAnimationDuration,
      exitAnimationDuration,
    });

    // Refresh data after the dialog is closed
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchUsers();
      }
    });
  }

openUpdateDialog(enterAnimationDuration: string, exitAnimationDuration: string, ID: number): void {
    const dialogRef = this.dialog.open(UpdateUserComponent, {
      width: '800px',
      enterAnimationDuration,
      exitAnimationDuration,
      data: { userId: ID }
    });

    // Refresh data after the dialog is closed
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchUsers();
      }
    });
  }

  DeleteUser(userId: number): void {
    this.http.delete<{ statusCode: number, message: string }>(`https://localhost:7106/api/User/Delete/${userId}`)
      .subscribe(
        response => {
          if (response.statusCode === 200) {
            this.snackBar.open('User deleted successfully!', 'Close', { duration: 3000 });
            this.fetchUsers(); // Refresh the user list after deletion
          } else {
            this.snackBar.open(`Failed to delete user: ${response.message}`, 'Close', { duration: 3000 });
          }
        },
        error => {
          console.error('Failed to delete user', error);
          this.snackBar.open('Failed to delete user', 'Close', { duration: 3000 });
        }
      );
  }

}
