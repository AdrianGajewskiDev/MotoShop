import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AdvertisementDetailsModel } from 'src/app/shared/models/advertisements/advertisementDetails.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';
import { Motocycle } from "../../shared/models/advertisements/Items/motocycle.model"
import { Car } from "../../shared/models/advertisements/Items/car.model"
import { ItemType } from "../../shared/Helpers/item.type"


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
  itemType: ItemType;
  item: "Car" | "Motocycle" = "Car";

  ngOnInit(): void {
    this.advertisementsService.getByID(this.data.AdvertisementID).subscribe((res) => {
      this.model = res;
      if (this.model.ShopItem.ItemType == "Motocycle") {
        this.item = "Motocycle";
        this.itemType = this.model.ShopItem as Motocycle;
      }
      else {
        this.item = "Car";
        this.itemType = this.model.ShopItem as Car;
      }
    }, error => { console.log(error); });
  }
}
