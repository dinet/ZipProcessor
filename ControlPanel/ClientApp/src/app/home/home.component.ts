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
  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }
  openModal() {
    const dialogConfig = new MatDialogConfig();
    if (this.fileToUpload != undefined) {
      const dialogRef = this.dialog.open(LoginDialogComponent, dialogConfig);
      dialogRef.afterClosed().subscribe(result => {
        const formData = new FormData();
        formData.append(this.fileToUpload.name, this.fileToUpload);
        formData.append('username', result.username);
        formData.append('password', result.password);
        this.http.post(this.baseUrl + 'api/files/UploadFile', formData).subscribe(
          data => {
            var da : any = data;
            if (!da.isSuccessStatusCode) {
              alert(da.reasonPhrase);
            }
          },
          error => console.log(error)
        );
      });
    }
  }
}
