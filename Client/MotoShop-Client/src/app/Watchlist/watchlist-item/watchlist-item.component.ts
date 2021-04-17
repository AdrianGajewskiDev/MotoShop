import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { buildImagePath } from 'src/app/shared/Helpers/buildProfileImagePath';
import { WatchListItemModel } from 'src/app/shared/models/Watchlist/watchlistItemModel';

@Component({
  selector: 'app-watchlist-item',
  templateUrl: './watchlist-item.component.html',
  styleUrls: ['./watchlist-item.component.sass']
})
export class WatchlistItemComponent implements OnInit {

  @Input() Model: WatchListItemModel;

  public baseServrResourcesPath = "wwwroot/resources/images/";
  public slides: string[] = [];

  public pinDateTransformed;
  constructor(private datePipe: DatePipe) { }

  ngOnInit(): void {
    for (let url of this.Model.ImageUrls)
      this.slides.push(this.buildImageUrl(this.baseServrResourcesPath + url));

    this.pinDateTransformed = this.datePipe.transform(this.Model.PinDate, "dd-mm-yyyy")
  }


  buildImageUrl(url) {
    return buildImagePath(url);
  }
}
