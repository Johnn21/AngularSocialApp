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
    ChatModalComponent
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
    ModalModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }