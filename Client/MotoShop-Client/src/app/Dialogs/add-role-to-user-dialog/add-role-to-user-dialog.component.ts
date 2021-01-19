import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ServiceLocator } from 'src/app/shared/services/locator.service';
import { UserService } from 'src/app/shared/services/user.service';

export interface DialogData {
  UserID: string;
}


@Component({
  selector: 'app-add-role-to-user-dialog',
  templateUrl: './add-role-to-user-dialog.component.html',
  styleUrls: ['./add-role-to-user-dialog.component.sass']
})
export class AddRoleToUserDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AddRoleToUserDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.userService = ServiceLocator.injector.get(UserService);
    this.toastr = ServiceLocator.injector.get(ToastrService);
  }

  public role: string;
  public showLoadingSpinner = false;

  userService: UserService;
  toastr: ToastrService;

  submit() {
    this.showLoadingSpinner = true;
    if (this.role != null && this.data.UserID != null) {
      this.userService.addRole(this.data.UserID, this.role).subscribe(
        () => {
          this.showLoadingSpinner = false;
          this.toastr.info(`Role ${this.role} added to user`, "Success!");
          this.dialogRef.close();
        },
        error => {
          this.showLoadingSpinner = false;
          console.log(error.error);
          this.toastr.error(error.error);
        }
      );
    }

  }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
