import { Component, Inject, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/shared/models/administration/user.model';
import { Advertisement } from 'src/app/shared/models/advertisements/advertisement.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';

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
  }

  adService: AdvertisementsService;
  toastr: ToastrService;
  advertisements: Advertisement[] = [];
  colors: ThemePalette = "accent";

  ngOnInit(): void {


    this.adService.getAllByUserID(this.data.User.Id).subscribe(
      (res: any) => {
        this.advertisements = res.Advertisements;
      },
      error => this.toastr.error(error.message)
    );
  }

}
