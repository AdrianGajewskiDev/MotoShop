import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { buildImagePath } from '../shared/Helpers/buildProfileImagePath';
import { AdvertisementDetailsModel } from '../shared/models/advertisements/advertisementDetails.model';
import { AdvertisementsService } from '../shared/services/advertisements.service';

@Component({
  selector: 'app-advertisement-details',
  templateUrl: './advertisement-details.component.html',
  styleUrls: ['./advertisement-details.component.sass']
})
export class AdvertisementDetailsComponent implements OnInit {

  private id;

  public model: AdvertisementDetailsModel;
  public showLoadingSpinner = true;
  public imageUrl;
  public phoneNumber: string;

  constructor(private routes: ActivatedRoute,
    private advertisementsService: AdvertisementsService,
    private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.id = this.routes.snapshot.params["id"];

    this.advertisementsService.getByID(this.id).subscribe(res => {
      this.showLoadingSpinner = false;
      this.model = res;
      this.imageUrl = buildImagePath(res.ShopItem.ImageUrl);
      this.model.Placed = this.datePipe.transform(this.model.Placed, "dd-mm-yyyy")
      this.model.ShopItem.YearOfProduction = this.datePipe.transform(this.model.ShopItem.YearOfProduction, "yyyy")
      this.phoneNumber = this.model.Author.PhoneNumber.substring(0, 2) + "x-xxx-xxx";
    }, error => {
      this.showLoadingSpinner = false;
      console.log(error);
    });
  }
  showPhoneNumber() {
    this.phoneNumber = this.model.Author.PhoneNumber;
  }
}
