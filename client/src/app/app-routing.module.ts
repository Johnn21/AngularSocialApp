import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivateAccountComponent } from './account/activate-account/activate-account.component';
import { ForgotPasswordComponent } from './account/forgot-password/forgot-password.component';
import { LoginComponent } from './account/login/login.component';
import { RegisterComponent } from './account/register/register.component';
import { AddPostComponent } from './add-post/add-post.component';
import { FriendRequestsComponent } from './friend-requests/friend-requests.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ProfileComponent } from './profile/profile.component';
import { UpdateProfileComponent } from './profile/update-profile/update-profile.component';
import { StartPageComponent } from './start-page/start-page.component';
import { AuthGuard } from './_guards/auth.guard';
import { UnauthGuard } from './_guards/unauth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'start-page',
    pathMatch: 'full'
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'home', component: HomeComponent},
      {path: 'profile/:username', component: ProfileComponent},
      {path: 'update-profile/:username', component: UpdateProfileComponent},
      {path: 'friend-requests/:username', component: FriendRequestsComponent},
      {path: 'add-post', component: AddPostComponent}
    ]
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [UnauthGuard],
    children: [
      {path: 'login', component: LoginComponent},
      {path: 'start-page', component: StartPageComponent},
      {path: 'register', component: RegisterComponent},
      {path: 'activate-account/:confirmationToken/:username', component: ActivateAccountComponent},
      {path: 'forgot-password', component: ForgotPasswordComponent}
    ]
  },
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
