import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Output() onFileUploaded = new EventEmitter<any>();
  constructor() { }

  ngOnInit(): void {
  }
  afuConfig = {
    multiple: false,
    formatsAllowed: ".jpg,.png,.xlsx,.pdf,.docx",
    maxSize: "20",
    uploadAPI: {
      url:this.baseUrl + 'Upload/upload',
      method:"POST",
      headers: {
        'Accept': '*/*',
         "Authorization" : `Bearer ${localStorage.getItem('token')}`
      },
    },
    hideProgressBar: false,
    hideResetBtn: true,
    hideSelectBtn: false,
    fileNameIndex: true,
    replaceTexts: {
      selectFileBtn: 'Select File',
      resetBtn: 'Reset',
      uploadBtn: 'Upload',
      dragNDropBox: 'Drag N Drop',
      attachPinBtn: 'Attach Files...',
      afterUploadMsg_success: 'Successfully Uploaded !',
      afterUploadMsg_error: 'Upload Failed !',
      sizeLimit: 'Size Limit'
    }
};


  docUpload(event) {
    this.onFileUploaded.emit(event.body.id);
    console.log('ApiResponse -> docUpload -> Event: ',event.body.id);
  }

}
