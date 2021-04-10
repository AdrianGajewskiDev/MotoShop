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

  public slides = [];
  ngOnInit(): void {
    for (const image of this.advertisement.ImageUrl) {
      console.log(this.buildImageUrl(image));

      this.slides.push({ image: this.buildImageUrl(image) });
    }
  }

  buildImageUrl(url) {
    return buildImagePath(url);
  }

  goToDetails() {
    this.router.navigateByUrl("/details/" + this.advertisement.Id);
  }
}
