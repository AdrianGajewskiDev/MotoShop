import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Advertisement } from "../models/advertisements/advertisement.model";
import { serverGetAllAdvertisementsByUserIDUrl } from "../server-urls";

@Injectable()
export class AdvertisementsService {
    constructor(private httpClient: HttpClient) { }

    getAllByUserID(id: string) {
        return this.httpClient.get<Advertisement[]>(serverGetAllAdvertisementsByUserIDUrl + "/" + id);
    }
}