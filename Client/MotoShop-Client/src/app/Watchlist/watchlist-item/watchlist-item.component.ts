import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { buildImagePath } from 'src/app/shared/Helpers/buildProfileImagePath';
import { WatchListItemModel } from 'src/app/shared/models/Watchlist/watchlistItemModel';
import { WatchlistService } from 'src/app/shared/services/watchlist.service';

@Component({
  selector: 'app-watchlist-item',
  templateUrl: './watchlist-item.component.html',
  styleUrls: ['./watchlist-item.component.sass']
})
export class WatchlistItemComponent implements OnInit {

  @Input() Model: WatchListItemModel;

  public baseServrResourcesPath = "wwwroot/resources/images/";
  public slides: string[] = [];

  public btnMessage = "Unwatch";

  public pinDateTransformed;
  constructor(private datePipe: DatePipe,
    private router: Router,
    private service: WatchlistService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    for (let url of this.Model.ImageUrls)
      this.slides.push(this.buildImageUrl(this.baseServrResourcesPath + url));

    this.pinDateTransformed = this.datePipe.transform(this.Model.PinDate, "dd-mm-yyyy")
  }


  buildImageUrl(url) {
    return buildImagePath(url);
  }

  goToDetails() {
    this.router.navigateByUrl("/details/" + this.Model.ItemId);
  }

  deleteItem() {
    this.btnMessage = "Processing..."
    this.service.deleteWatchlistItem(this.Model.Id).subscribe(() => {
      this.toastr.success("Successfully deleted from your watchlist")
      this.btnMessage = "Done";
    },
      error => console.log(error));
  }
}
