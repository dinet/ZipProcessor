import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent implements OnInit {

  username: string;
  password: string;

  modalTitle: string;
  constructor(private dialogRef: MatDialogRef<LoginDialogComponent>) {
  }
  submit() {
    const credentials = {
      username: this.username,
      password: this.password
    }
    this.dialogRef.close(credentials);
  }
  ngOnInit() {
  }

}
