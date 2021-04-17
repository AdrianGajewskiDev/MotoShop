import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddAdvertisementComponent } from './add-advertisement/add-advertisement.component';
import { AdministrationPanelComponent } from './administration/administration-panel/administration-panel.component';
import { AdvertisementDetailsComponent } from './advertisement-details/advertisement-details.component';
import { HomeComponent } from './home/home.component';
import { ConfirmationComponent } from './identity/confirmation-component/confirmation-component.component';
import { ForgotPasswordComponent } from './identity/forgot-password/forgot-password.component';
import { IdentityPlaceholderComponent } from './identity/identity-placeholder/identity-placeholder.component';
import { LoginComponent } from './identity/login/login.component';
import { RegisterComponent } from './identity/register/register.component';
import { UserProfileComponent } from './identity/user-profile/user-profile.component';
import { AdministratorGuard } from './shared/Guards/administrator.guard';
import { AuthenticationGuard } from './shared/Guards/authentication.guard';
import { WatchlistComponent } from './Watchlist/watchlist/watchlist.component';


const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "identity",
    component: IdentityPlaceholderComponent,
    children: [
      {
        path: '',
        component: LoginComponent,
        outlet: "identity"
      },
      {
        path: 'register',
        component: RegisterComponent,
        outlet: "identity"
      },
      {
        path: 'login',
        component: LoginComponent,
        outlet: "identity"
      }
    ]
  },
  {
    path: "userProfile",
    component: UserProfileComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: "confirmation/:type?",
    component: ConfirmationComponent
  },
  {
    path: 'forgotPassword',
    component: ForgotPasswordComponent
  },
  {
    path: "administrator",
    component: AdministrationPanelComponent,
    canActivate: [AdministratorGuard]
  },
  {
    path: "new-advertisement",
    component: AddAdvertisementComponent,
    canActivate: [AuthenticationGuard]
  },
  {
    path: "details/:id",
    component: AdvertisementDetailsComponent,
  },
  {
    path: "watchlist",
    component: WatchlistComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
