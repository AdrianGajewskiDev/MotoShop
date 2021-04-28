import { DatePipe } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { buildImagePath } from '../shared/Helpers/buildProfileImagePath';
import { AdvertisementDetailsModel } from '../shared/models/advertisements/advertisementDetails.model';
import { AdvertisementsService } from '../shared/services/advertisements.service';
import { IdentityService } from '../shared/services/identity.service';
import { WatchlistService } from '../shared/services/watchlist.service';

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
  private baseServrResourcesPath = "wwwroot/resources/images/";

  public btnMessage = "Watch";

  public large = { width: "500px", height: "300px" };
  public medium = { width: "400px", height: "200px" };

  public currentImageSliderWidth = this.large;
  public innerWidth;
  constructor(private routes: ActivatedRoute,
    private advertisementsService: AdvertisementsService,
    private datePipe: DatePipe,
    private watchlistService: WatchlistService,
    private toastrService: ToastrService,
    private identityService: IdentityService) { }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.innerWidth = window.innerWidth;

    if (this.innerWidth <= 550) {
      this.currentImageSliderWidth = this.medium;
    }
  }

  public slides: string[] = [];
  ngOnInit(): void {

    this.id = this.routes.snapshot.params["id"];

    this.advertisementsService.getByID(this.id).subscribe(res => {
      this.showLoadingSpinner = false;
      this.model = res;
      this.imageUrl = buildImagePath(res.ShopItem.ImageUrl);
      this.model.Placed = this.datePipe.transform(this.model.Placed, "dd-mm-yyyy")
      this.model.ShopItem.YearOfProduction = this.datePipe.transform(this.model.ShopItem.YearOfProduction, "yyyy")
      this.phoneNumber = this.model.Author.PhoneNumber.substring(0, 2) + "x-xxx-xxx";
      for (const image of this.model.ImageUrls) {
        this.slides.push(this.buildImageUrl(this.baseServrResourcesPath + image));
      }

      console.log(this.model);

    }, error => {
      this.showLoadingSpinner = false;
      console.log(error);
    });
  }
  showPhoneNumber() {
    this.phoneNumber = this.model.Author.PhoneNumber;
  }
  buildImageUrl(url) {
    return buildImagePath(url);
  }

  addToWatchlist() {
    this.btnMessage = "Processing..."
    this.watchlistService.addToWatchlist(this.model.ID).subscribe(res => {
      this.btnMessage = "Watch";
      this.toastrService.success("Successfully added item to your watchlist")
    }, error => {
      console.log(error);

    });
  }
  isCurrentUserOwner() {
    if (this.identityService.isSignedIn)
      return this.identityService.getUserID === this.model.AuthorID;
    else
      return false;
  }
}
