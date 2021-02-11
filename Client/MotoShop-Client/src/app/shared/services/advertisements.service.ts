import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UpdateDataResult } from "src/app/Dialogs/edit-advertisement-dialog/edit-advertisement-dialog.component";
import { Advertisement } from "../models/advertisements/advertisement.model";
import { AdvertisementDetailsModel } from "../models/advertisements/advertisementDetails.model";
import { serverDeleteAdvertisementUrl, serverGetAllAdvertisementsByUserIDUrl, serverGetAllAdvertisementsUrl, serverUpdateAdvertisementUrl } from "../server-urls";

@Injectable()
export class AdvertisementsService {
    constructor(private httpClient: HttpClient) { }

    getAllByUserID(id: string) {
        return this.httpClient.get<Advertisement[]>(serverGetAllAdvertisementsByUserIDUrl + "/" + id);
    }

    getByID(id: number) {
        return this.httpClient.get<AdvertisementDetailsModel>(serverGetAllAdvertisementsUrl + id);
    }

    delete(id: number) {
        return this.httpClient.delete(serverDeleteAdvertisementUrl + id);
    }

    update(data: UpdateDataResult[]) {

        let updateDataModel = {
            dataModels: data
        }
        return this.httpClient.put(serverUpdateAdvertisementUrl, updateDataModel);
    }
}