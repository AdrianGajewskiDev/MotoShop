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
import { buildProfileImagePath } from "../../shared/Helpers/buildProfileImagePath"
import { UploadService } from 'src/app/shared/services/upload.service';
import { error } from 'protractor';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.sass']
})
export class UserProfileComponent implements OnInit {

  constructor(private userService: UserService,
    private fb: FormBuilder,
    private mapper: FormsMapper,
    private toastr: ToastrService,
    private uploadService: UploadService) { }

  public userData: UserProfileDataModel;
  public showError: boolean = false;
  public editUserDataForm: FormGroup;
  public editPasswordForm: FormGroup;

  public showLoadingSpinner: boolean = true;
  public showImageLoadingSpinner: boolean = true;
  public showUpdatingError: boolean = false;
  public errorMessage: string = "";
  public imageUrl: string = "";

  public color = "Black";

  ngOnInit(): void {
    this.editUserDataForm = this.fb.group({
      name: [''],
      lastName: [''],
      email: ['', [Validators.email]],
      username: ['']
    })
    this.userService.getUserProfileData().subscribe(
      (res: ApiResponse<UserProfileDataModel>) => {
        this.userData = res.ResponseContent;
        this.showLoadingSpinner = false;
        this.showImageLoadingSpinner = false;
        if (this.userData.IsExternal)
          this.imageUrl = this.userData.ImageUrl;
        else
          this.imageUrl = buildProfileImagePath(this.userData.ImageUrl);
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

  editUserData(): void {
    this.showLoadingSpinner = true;

    if (isEmpty(this.editUserDataForm))
      return;

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

  ///TODO: implement these once the uploading profile picture functionality is working
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


  editPhoto(): void { }
  deletePhoto(): void { }
}
