import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-post-photo',
  templateUrl: './add-post-photo.component.html',
  styleUrls: ['./add-post-photo.component.css']
})
export class AddPostPhotoComponent implements OnInit {
  hasBaseDropzoneOver = false;
  uploader: FileUploader;
  baseUrl = environment.apiUrl;
  user: User;

  constructor(private accountService: AccountService, private router: Router) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'post/add-photo-to-post',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {

      if(this.uploader.queue.length > 1) {
        this.uploader.queue[0].remove();
      }

      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        this.router.navigate(['home'], { state: { post: 'post-added' } });
      }
    }
  }

  navigateHome() {
    this.router.navigate(['home'], { state: { post: 'post-added' } });
  }

}
