import { Component, OnInit } from '@angular/core';
import { WatchlistModel } from 'src/app/shared/models/Watchlist/watchlistModel';
import { WatchlistService } from 'src/app/shared/services/watchlist.service';

@Component({
  selector: 'app-watchlist',
  templateUrl: './watchlist.component.html',
  styleUrls: ['./watchlist.component.sass']
})
export class WatchlistComponent implements OnInit {

  constructor(private service: WatchlistService) { }

  public model: WatchlistModel;
  public showLoadingSpinner = true;
  public hasItemsInWatchlist = false;

  ngOnInit(): void {
    this.service.getWatchlist().subscribe((res: WatchlistModel) => {
      this.model = res;
      this.showLoadingSpinner = false;
      if (this.model.Items != null)
        this.hasItemsInWatchlist = true;
    },
      error => {
        console.log(error);
        this.showLoadingSpinner = false;
      });
  }

}
