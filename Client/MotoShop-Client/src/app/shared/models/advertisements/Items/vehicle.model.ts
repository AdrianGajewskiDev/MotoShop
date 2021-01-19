import { ShopItem } from "../shopItem.model";

export class Vehicle extends ShopItem {
    HorsePower: number;
    FuelConsumption: number;
    Acceleration: number;
    Lenght: number;
    Width: number;
    CubicCapacity: number;
    Fuel: string;
    YearOfProduction: Date;
}
