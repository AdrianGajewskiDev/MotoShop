import { UserProfileDataModel } from "../user/userProfileData.model";

export class AdvertisementDetailsModel {
    ID: number;
    Title: string;
    Description: string;
    Placed: string;
    AuthorID: string;
    Author: UserProfileDataModel;
    ShopItem: any;
    ImageUrls: string[];
}