import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UpdateDataResult } from "src/app/Dialogs/edit-advertisement-dialog/edit-advertisement-dialog.component";
import { Advertisement } from "../models/advertisements/advertisement.model";
import { AdvertisementDetailsModel } from "../models/advertisements/advertisementDetails.model";
import { serverAddAdvertisementUrl, serverDeleteAdvertisementUrl, serverGetAllAdvertisementsByUserIDUrl, serverGetAllAdvertisementsUrl, serverGetTopAdvertisementsUrl, serverUpdateAdvertisementUrl } from "../server-urls";

@Injectable()
export class AdvertisementsService {
    constructor(private httpClient: HttpClient) { }

    getAllByUserID(id: string) {
        return this.httpClient.get<Advertisement[]>(serverGetAllAdvertisementsByUserIDUrl + "/" + id);
    }

    getByID(id: number) {
        return this.httpClient.get<AdvertisementDetailsModel>(serverGetAllAdvertisementsUrl + "/" + id);
    }

    delete(id: number) {
        return this.httpClient.delete(serverDeleteAdvertisementUrl + id);
    }

    update(data: UpdateDataResult[], id: number) {

        let updateDataModel = {
            dataModels: data
        }
        return this.httpClient.put(serverUpdateAdvertisementUrl + id, updateDataModel);
    }

    getAll(perPage?: number, pageNumber?: number) {
        if (!perPage && !pageNumber)
            return this.httpClient.get(serverGetAllAdvertisementsUrl);
        else {
            return this.httpClient.get(serverGetAllAdvertisementsUrl + "?page=" + pageNumber + "&pageSize=" + perPage);
        }
    }

    getTopThree() {
        return this.httpClient.get(serverGetTopAdvertisementsUrl);
    }

    addAdvertisement(model) {
        return this.httpClient.post(serverAddAdvertisementUrl, model);
    }
}