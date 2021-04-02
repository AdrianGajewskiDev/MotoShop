import { NewAdvertisementBaseInfoModel } from "./newAdvertisementBaseInfoModel";
import { NewAdvertisementBaseModel } from "./newAdvertisementBaseModel";

export class NewCarAdvertisementModel extends NewAdvertisementBaseModel {
    BaseInfo: NewAdvertisementBaseInfoModel;
    CarBrand: string;
    CarModel: string;
    Gearbox: string;
    BodyType: string;
    Length: number;
    Width: number;
    Acceleration: number;
    NumberOfDoors: Date;
    NumberOfSeats: number;
}