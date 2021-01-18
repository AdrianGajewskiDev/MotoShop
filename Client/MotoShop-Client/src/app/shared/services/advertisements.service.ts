import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Advertisement } from "../models/advertisements/advertisement.model";
import { AdvertisementDetailsModel } from "../models/advertisements/advertisementDetails.model";
import { serverGetAllAdvertisementsByUserIDUrl, serverGetAllAdvertisementsUrl } from "../server-urls";

@Injectable()
export class AdvertisementsService {
    constructor(private httpClient: HttpClient) { }

    getAllByUserID(id: string) {
        return this.httpClient.get<Advertisement[]>(serverGetAllAdvertisementsByUserIDUrl + "/" + id);
    }

    getByID(id: number) {
        return this.httpClient.get<AdvertisementDetailsModel>(serverGetAllAdvertisementsUrl + id);
    }
}