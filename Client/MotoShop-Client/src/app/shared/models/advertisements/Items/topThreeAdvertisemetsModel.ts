import { AdvertisementOverallDetails } from "../advertisementOverallDetails.model";

export class TopThreeAdvertisementsResult {
    SportCars: AdvertisementOverallDetails[];
    SuvCars: AdvertisementOverallDetails[];
    SedanCars: AdvertisementOverallDetails[];
}

export class TopThreeAdvertisementsRequestResult {
    Advertisements: TopThreeAdvertisementsResult;
}