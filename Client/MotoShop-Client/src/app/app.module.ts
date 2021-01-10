import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations"
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AngularMaterialModule } from "./modules/angular.material.module";
import { FlexLayoutModule } from "@angular/flex-layout";
import { ReactiveFormsModule, FormsModule } from "@angular/forms"
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http"
import { ToastrModule } from "../../node_modules/ngx-toastr"

//services
import { FormsMapper } from './shared/mapper/formsMapper';
import { IdentityService } from "./shared/services/identity.service"

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { IdentityPlaceholderComponent } from './identity/identity-placeholder/identity-placeholder.component';
import { RegisterComponent } from './identity/register/register.component';
import { LoginComponent } from './identity/login/login.component';
import { HomeComponent } from './home/home.component';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';
import { UserProfileComponent } from './identity/user-profile/user-profile.component';
import { AuthenticationInterceptor } from './shared/Interceptors/authentication.interceptor';
import { UserService } from './shared/services/user.service';
import { ConfirmationComponent } from './identity/confirmation-component/confirmation-component.component';
import { ForgotPasswordComponent } from './identity/forgot-password/forgot-password.component';
import { UploadService } from './shared/services/upload.service';
import { GoogleAuthenticationModule } from './modules/google.authentication.module';
import { ExternalSignInService } from './shared/services/externalSignIn.service';
import { AdministrationPanelComponent } from './administration-panel/administration-panel.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    IdentityPlaceholderComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    LoadingSpinnerComponent,
    UserProfileComponent,
    ConfirmationComponent,
    ForgotPasswordComponent,
    AdministrationPanelComponent
  ],
  imports: [
    AngularMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    GoogleAuthenticationModule

  ],
  providers: [
    IdentityService,
    FormsMapper,
    UserService,
    UploadService,
    ExternalSignInService,
    ///Interceptors
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
