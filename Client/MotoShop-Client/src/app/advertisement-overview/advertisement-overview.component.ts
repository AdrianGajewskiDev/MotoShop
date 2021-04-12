import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { buildImagePath } from '../shared/Helpers/buildProfileImagePath';
import { AdvertisementOverallDetails } from '../shared/models/advertisements/advertisementOverallDetails.model';

@Component({
  selector: 'app-advertisement-overview',
  templateUrl: './advertisement-overview.component.html',
  styleUrls: ['./advertisement-overview.component.sass']
})
export class AdvertisementOverviewComponent implements OnInit {

  constructor(private router: Router) { }

  @Input() public advertisement: AdvertisementOverallDetails

  public baseServrResourcesPath = "wwwroot/resources/images/";

  public slides: string[] = [];
  ngOnInit(): void {
    for (const image of this.advertisement.ImageUrl) {

      this.slides.push(this.buildImageUrl(this.baseServrResourcesPath + image));
      console.log(this.buildImageUrl(this.baseServrResourcesPath + image));

    }
  }

  buildImageUrl(url) {
    return buildImagePath(url);
  }

  goToDetails() {
    this.router.navigateByUrl("/details/" + this.advertisement.Id);
  }
}
