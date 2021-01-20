import { ShopItem } from "./shopItem.model"

export interface Advertisement {
    ID: number;
    Title: string;
    Description: string;
    Placed: string;
    AuthorID: string;
    ShopItem: ShopItem;
}