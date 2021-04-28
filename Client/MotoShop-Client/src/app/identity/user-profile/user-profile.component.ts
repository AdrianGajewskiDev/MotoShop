import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from 'src/app/shared/apiResponse';
import { passwordsMatchesValidator } from 'src/app/shared/custom-validators';
import { FormsMapper } from 'src/app/shared/mapper/formsMapper';
import { UpdateUserPasswordModel } from 'src/app/shared/models/user/updateUserPassword.model';
import { UpdateUserProfileDataModel } from 'src/app/shared/models/user/updateUserProfileData.model';
import { UserProfileDataModel } from 'src/app/shared/models/user/userProfileData.model';
import { passwordValidators } from 'src/app/shared/password-validators';
import { UserService } from 'src/app/shared/services/user.service';
import { isEmpty } from '../../shared/Helpers/formGroupHelpers'
import { buildImagePath } from "../../shared/Helpers/buildProfileImagePath"
import { UploadService } from 'src/app/shared/services/upload.service';
import { Router } from '@angular/router';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { IdentityService } from 'src/app/shared/services/identity.service';
import { MatTableDataSource } from '@angular/material/table';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.sass']
})
export class UserProfileComponent implements OnInit {

  constructor(public userService: UserService,
    private fb: FormBuilder,
    private mapper: FormsMapper,
    private toastr: ToastrService,
    private uploadService: UploadService,
    private router: Router,
    private adsService: AdvertisementsService,
    private identityService: IdentityService,
    private datePipe: DatePipe) { }

  public adsDataSource;

  public userData: UserProfileDataModel;
  public copiedUserData;
  public showError: boolean = false;
  public hasAnyAdvertisements: boolean = false;
  public editUserDataForm: FormGroup;
  public editPasswordForm: FormGroup;

  public showLoadingSpinner: boolean = true;
  public showImageLoadingSpinner: boolean = true;
  public showUpdatingError: boolean = false;
  public errorMessage: string = "";
  public imageUrl: string = "";

  public color = "Black";
  public displayedColumns: string[] = ['Title', 'Price', 'Placed'];

  ngOnInit(): void {
    this.getAds();

    this.editUserDataForm = this.fb.group({
      name: [],
      lastName: [''],
      email: ['', [Validators.email]],
      username: [''],
      phoneNumber: ['']
    })
    this.userService.getUserProfileData().subscribe(
      (res: ApiResponse<UserProfileDataModel>) => {
        this.userData = res.ResponseContent;
        this.copiedUserData = { ...this.userData };

        this.showLoadingSpinner = false;
        this.showImageLoadingSpinner = false;

        if (this.userData.IsExternal)
          this.imageUrl = this.userData.ImageUrl;
        else
          this.imageUrl = buildImagePath(this.userData.ImageUrl);


        console.log(this.imageUrl);


      },
      (error) => {

        console.log(error);
        this.showError = true;
      }

    )
    this.editPasswordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      password: ['', passwordValidators],
      confirmPassword: ['', Validators.required]
    },
      {
        validators: passwordsMatchesValidator
      });
  }

  fetchUserProfileDataToForm() {
    this.editUserDataForm.setValue({
      name: this.userData.Name,
      lastName: this.userData.LastName,
      email: this.userData.Email,
      username: this.userData.UserName,
      phoneNumber: this.userData.PhoneNumber
    });
  }

  editUserData(): void {
    this.showLoadingSpinner = true;

    let model = this.mapper.map<UpdateUserProfileDataModel>(new UpdateUserProfileDataModel(), this.editUserDataForm);

    this.userService.updateUserProfile(model).subscribe(
      res => {
        if (model.email != "")
          this.toastr.info("We have sent verification link to your email adress, click the link in the message to change your email")

        this.showLoadingSpinner = false;
        window.location.reload();
      },
      error => {
        if (error.status == 400) {
          this.showLoadingSpinner = false;
          this.showUpdatingError = true;
          this.errorMessage = error.error.Message;
        }
      }
    );
  }
  changePassword() {
    let model = this.mapper.map<UpdateUserPasswordModel>(new UpdateUserPasswordModel(), this.editPasswordForm);
    model.newPassword = this.editPasswordForm.get('password').value;
    this.userService.updatePassword(model).subscribe(res => {
      this.toastr.info("We have sent verification link to your email adress, click the link in the message to reset your password")
    },
      error => this.toastr.error("Ups, something went wrong while trying to reset the password, please try again"))

  }
  changePhoto(): void {
    this.showImageLoadingSpinner = true;
    const inputNode: any = document.querySelector('#file');

    this.uploadService.uploadImage(inputNode.files[0]).subscribe(
      res => {
        this.showImageLoadingSpinner = false;
        window.location.reload();
      },
      error => this.toastr.error(error.error.message));
  }
  goToAdminPanel() {
    this.router.navigateByUrl("/administrator");
  }
  editPhoto(): void { }
  deletePhoto(): void { }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.adsDataSource.filter = filterValue.trim().toLowerCase();
  }
  getAds() {
    this.showLoadingSpinner = true;
    this.errorMessage = "Processing...";
    if (!this.adsDataSource) {
      this.adsService.getAllByUserID(this.identityService.getUserID).subscribe(
        (res: any) => {
          if (res.Advertisements.length > 0) {
            res.Advertisements.forEach(element => {
              element.Placed = this.datePipe.transform(element.Placed, "yyyy-MM-dd")
            });
            this.adsDataSource = new MatTableDataSource(res.Advertisements);
            this.hasAnyAdvertisements = true;
          }
          else
            this.errorMessage = "You don't have any advertisements";
        },
        error => {
          this.showLoadingSpinner = false;
          console.log(error);
        }
      )
    }
  }

  setInputValue(control: HTMLInputElement, value) {
    control.value = value;
  }

  updateUserDataLocaly(dataType: string, newData) {

    switch (dataType) {
      case "name": {
        this.userData.Name = newData;
      } break;
      case "lastName": {
        this.userData.LastName = newData;
      } break;
      case "email": {
        this.userData.Email = newData;
      } break;
      case "phoneNumber": {
        this.userData.PhoneNumber = newData;
      } break;
      case "username": {
        this.userData.UserName = newData;
      } break;
    }
  }

  isUpdatingDataRequestAllowed(): boolean {
    let flag = false;

    if (JSON.stringify(this.userData) !== JSON.stringify(this.copiedUserData))
      flag = true;

    console.log(flag);
    return flag;
  }

  goToDetails(id) {
    this.router.navigateByUrl("/details/" + id);
  }
}
