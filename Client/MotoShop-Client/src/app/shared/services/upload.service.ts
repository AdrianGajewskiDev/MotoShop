import { HttpClient } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { serverAdvertisementImageUploadUrl, serverImageUploadUrl } from "../server-urls"

@Injectable()
export class UploadService {
    constructor(private client: HttpClient) { }

    uploadImage(image: File) {

        const data = new FormData();

        data.append("image", image);

        return this.client.post(serverImageUploadUrl, data);
    }

    uploadAdvertisementImage(image: File, param: any) {

        const data = new FormData();

        data.append("image", image);

        return this.client.post(serverAdvertisementImageUploadUrl + "/" + param, data);
    }

}