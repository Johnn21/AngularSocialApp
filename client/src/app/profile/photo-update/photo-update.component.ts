import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { Photo } from 'src/app/models/photo';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-update',
  templateUrl: './photo-update.component.html',
  styleUrls: ['./photo-update.component.css']
})
export class PhotoUpdateComponent implements OnInit {
  @Input() currentUser: Member;
  uploader: FileUploader;
  baseUrl = environment.apiUrl;
  user: User;
  hasBaseDropzoneOver = false;

  constructor(private accountService: AccountService, private memberService: MemberService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe(() => {
      this.currentUser.photos = this.currentUser.photos.filter(x => x.id !== photoId);
      this.toastr.success('Photo deleted');
    })
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.currentUser.photoUrl = photo.url;
      this.currentUser.photos.forEach(p => {
        if (p.isMain) p.isMain = false;
        if (p.id === photo.id) p.isMain = true;
      });
    })
  } 

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'member/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.currentUser.photos.push(photo);
        if (photo.isMain) {
          this.user.photoUrl = photo.url;
          this.currentUser.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    }
  }

}
