import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FileUploadModule } from 'ng2-file-upload';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ModalModule } from 'ngx-bootstrap/modal';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDatepickerModule.forRoot(),
    ToastrModule.forRoot(),
    TabsModule.forRoot(),
    BsDropdownModule.forRoot(),
    FileUploadModule,
    NgxGalleryModule,
    ModalModule.forRoot()
  ],
  exports: [
    BsDatepickerModule,
    ToastrModule,
    TabsModule,
    BsDropdownModule,
    FileUploadModule,
    NgxGalleryModule,
    ModalModule
  ]
})
export class SharedModule { }
