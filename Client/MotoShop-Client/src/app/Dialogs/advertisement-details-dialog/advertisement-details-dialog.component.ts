import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AdvertisementDetailsModel } from 'src/app/shared/models/advertisements/advertisementDetails.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';

interface DialogData {
  AdvertisementID: number;
}

@Component({
  selector: 'app-advertisement-details-dialog',
  templateUrl: './advertisement-details-dialog.component.html',
  styleUrls: ['./advertisement-details-dialog.component.sass']
})
export class AdvertisementDetailsDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AdvertisementDetailsDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.advertisementsService = ServiceLocator.injector.get(AdvertisementsService);
  }

  model: AdvertisementDetailsModel;
  advertisementsService: AdvertisementsService;

  ngOnInit(): void {
    this.advertisementsService.getByID(this.data.AdvertisementID).subscribe((res) => {
      console.log(res);
      this.model = res;
    }, error => { console.log(error); });
  }

}
