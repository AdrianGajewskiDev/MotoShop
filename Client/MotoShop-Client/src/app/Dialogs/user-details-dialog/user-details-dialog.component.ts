import { Component, Inject, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/shared/models/administration/user.model';
import { Advertisement } from 'src/app/shared/models/advertisements/advertisement.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';
import { UserService } from 'src/app/shared/services/user.service';
import { AddRoleToUserDialogComponent } from '../add-role-to-user-dialog/add-role-to-user-dialog.component';
import { AdvertisementDetailsDialogComponent } from '../advertisement-details-dialog/advertisement-details-dialog.component';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';

interface DialogData {
  User: User;
}

@Component({
  selector: 'app-user-details-dialog',
  templateUrl: './user-details-dialog.component.html',
  styleUrls: ['./user-details-dialog.component.sass']
})
export class UserDetailsDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<UserDetailsDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.adService = ServiceLocator.injector.get(AdvertisementsService);
    this.toastr = ServiceLocator.injector.get(ToastrService);
    this.userService = ServiceLocator.injector.get(UserService);
    this.dialog = ServiceLocator.injector.get(MatDialog);
  }
  public displayedColumns: string[] = ['Id', 'Title', 'Price', 'Placed'];
  public dataSource;

  public showLoadingSpinner = false;

  adService: AdvertisementsService;
  toastr: ToastrService;
  userService: UserService;
  dialog: MatDialog;
  advertisements: Advertisement[] = [];
  colors: ThemePalette = "accent";

  ngOnInit(): void {
    this.adService.getAllByUserID(this.data.User.Id).subscribe(
      (res: any) => {
        this.advertisements = res.Advertisements;
        this.dataSource = new MatTableDataSource(this.advertisements);
      },
      error => this.toastr.error(error.message)
    );
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  addRole(id: string) {
    this.showLoadingSpinner = true;
    this.dialog.open(AddRoleToUserDialogComponent,
      {
        width: '250px',
        data: {
          UserID: id
        }
      });

  }

  deleteUser(id: string) {
    this.dialog.open(ConfirmationDialogComponent, {
      width: '250px',
      data:
      {
        func: (s) => {
          if (s == false) {
            this.deleteUserCallback(id)
          }
        },
        run: false,
        message: "Are you sure deleting this user?"
      }
    });
  }

  deleteUserCallback(id: string): void {
    this.showLoadingSpinner = true;
    this.userService.deleteUser(id).subscribe(
      (res) => {
        this.showLoadingSpinner = false;
        this.toastr.info(`User with id ${id} was successfully deleted!`, "Success!");
        window.location.reload();
      },
      error => {
        this.showLoadingSpinner = false;
        this.toastr.error(error.error);
      }
    );
  }

  goToAdvertDetails(id: number) {
    this.dialog.open(AdvertisementDetailsDialogComponent, {
      width: '1000px',
      height: '600px',
      data: {
        AdvertisementID: id
      }
    });
  }
}
