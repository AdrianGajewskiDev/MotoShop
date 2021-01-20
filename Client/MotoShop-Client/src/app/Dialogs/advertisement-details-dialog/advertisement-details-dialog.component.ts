import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AdvertisementDetailsModel } from 'src/app/shared/models/advertisements/advertisementDetails.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';
import { Motocycle } from "../../shared/models/advertisements/Items/motocycle.model"
import { Car } from "../../shared/models/advertisements/Items/car.model"
import { ItemType } from "../../shared/Helpers/item.type"
import { DatePipe } from '@angular/common';


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
    this.datePipe = ServiceLocator.injector.get(DatePipe);
  }

  model: AdvertisementDetailsModel;
  advertisementsService: AdvertisementsService;
  datePipe: DatePipe;
  itemType: ItemType;
  item: string = "";
  car: Car;
  motocycle: Motocycle;

  ngOnInit(): void {
    this.advertisementsService.getByID(this.data.AdvertisementID).subscribe((res) => {
      this.model = res;
      if (this.model.ShopItem.ItemType == "Motocycle") {
        this.item = "Motocycle";
        this.motocycle = this.model.ShopItem as Motocycle;
        this.motocycle.YearOfProduction = this.datePipe.transform(this.motocycle.YearOfProduction, "yyyy")
      }
      else {
        this.item = "Car";
        this.car = this.model.ShopItem as Car;
        this.car.YearOfProduction = this.datePipe.transform(this.car.YearOfProduction, "yyyy-MM")

      }
      this.model.Placed = this.datePipe.transform(this.model.Placed, "yyyy-MM-dd");
    }, error => { console.log(error); });
  }
}
