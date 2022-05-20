import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from './_modules/shared.module';
import { RegisterComponent } from './account/register/register.component';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { LoginComponent } from './account/login/login.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { StartPageComponent } from './start-page/start-page.component';
import { ProfileComponent } from './profile/profile.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { UpdateProfileComponent } from './profile/update-profile/update-profile.component';
import { PhotoUpdateComponent } from './profile/photo-update/photo-update.component';
import { FriendRequestsComponent } from './friend-requests/friend-requests.component';
import { FriendsListComponent } from './home/friends-list/friends-list.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ChatModalComponent } from './_modals/chat-modal/chat-modal.component';
import { AddPostComponent } from './add-post/add-post.component';
import { AddPostInfoComponent } from './add-post/add-post-info/add-post-info.component';
import { AddPostPhotoComponent } from './add-post/add-post-photo/add-post-photo.component';
import { StepperComponent } from './stepper/stepper.component';
import { PostsWallComponent } from './home/posts-wall/posts-wall.component';
import { ProfileFriendsComponent } from './profile/profile-friends/profile-friends.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ActivateAccountComponent } from './account/activate-account/activate-account.component';
import { ForgotPasswordComponent } from './account/forgot-password/forgot-password.component';
import { ForgotPasswordSendEmailComponent } from './account/forgot-password/forgot-password-send-email/forgot-password-send-email.component';
import { ForgotPasswordConfirmCodeComponent } from './account/forgot-password/forgot-password-confirm-code/forgot-password-confirm-code.component';
import { ForgotPasswordNewComponent } from './account/forgot-password/forgot-password-new/forgot-password-new.component';
import { PostCommentComponent } from './home/posts-wall/post-comment/post-comment.component';
import { ProfilePostsComponent } from './profile/profile-posts/profile-posts.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterComponent,
    HomeComponent,
    LoginComponent,
    StartPageComponent,
    ProfileComponent,
    NotFoundComponent,
    UpdateProfileComponent,
    PhotoUpdateComponent,
    FriendRequestsComponent,
    FriendsListComponent,
    ChatModalComponent,
    AddPostComponent,
    AddPostInfoComponent,
    AddPostPhotoComponent,
    StepperComponent,
    PostsWallComponent,
    ProfileFriendsComponent,
    ActivateAccountComponent,
    ForgotPasswordComponent,
    ForgotPasswordSendEmailComponent,
    ForgotPasswordConfirmCodeComponent,
    ForgotPasswordNewComponent,
    PostCommentComponent,
    ProfilePostsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    FormsModule,
    ModalModule.forRoot(),
    PaginationModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
