import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Inject } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { LoginDialogComponent } from '../login-dialog/login-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  fileToUpload: File = null;
  username: string = null;
  password: string = null;
  baseUrl: string = '';
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, public dialog: MatDialog) {
    this.baseUrl = baseUrl;
  }

  openModal() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      id: 1,
      title: "Angular For Beginners"
    };
    const dialogRef = this.dialog.open(LoginDialogComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      console.log("Dialog was closed")
      console.log(result)
    });
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    const formData = new FormData();
    formData.append(this.fileToUpload.name, this.fileToUpload);
    formData.append('username', 'user_name');
    formData.append('password', 'password');
    this.http.post(this.baseUrl + 'api/files/UploadFile', formData).subscribe(
      data => console.log('success'),
      error => console.log(error)
    );
  }

}
