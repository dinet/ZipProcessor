import { Component } from '@angular/core';
import { HttpModule } from '@angular/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  fileToUpload: File = null;
  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);


    Observable<Response> ob = this.http.post(this.url, book, options); 
    console.log(this.fileToUpload);
  }

}
