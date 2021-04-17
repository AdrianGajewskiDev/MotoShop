import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { serverAddToWatchlistUrl, serverWatchlistUrl } from "../server-urls";

@Injectable()
export class WatchlistService {
    constructor(private httpClient: HttpClient) { }

    getWatchlist() {
        return this.httpClient.get(serverWatchlistUrl)
    }

    addToWatchlist(id: number) {
        return this.httpClient.post(serverAddToWatchlistUrl + "/" + id, null);
    }
}