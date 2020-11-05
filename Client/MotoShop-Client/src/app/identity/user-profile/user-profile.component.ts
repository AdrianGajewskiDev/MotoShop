import { Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/shared/apiResponse';
import { UserProfileDataModel } from 'src/app/shared/models/user/userProfileData.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.sass']
})
export class UserProfileComponent implements OnInit {

  constructor(private userService: UserService) { }

  public userData: UserProfileDataModel;
  public showError: boolean = false;

  ngOnInit(): void {

    this.userService.getUserProfileData().subscribe(
      (res: ApiResponse<UserProfileDataModel>) => {
        this.userData = res.ResponseContent;
        console.log(res);
      },
      (error) => {
        console.log(error);
        this.showError = true;
      }

    )
  }

}
