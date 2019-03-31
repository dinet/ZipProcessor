import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  fileToUpload: File = null;
  baseUrl: string = '';
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    const formData = new FormData();
    formData.append(this.fileToUpload.name, this.fileToUpload);
    this.http.post(this.baseUrl + 'api/files/UploadFile', formData).subscribe(
        data => console.log('success'),
        error => console.log(error)
      ); 
  }

}
