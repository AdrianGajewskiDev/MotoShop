import { UserProfileDataModel } from "../user/userProfileData.model";
import { Message } from "./message.model";

export class Conversation {
    Id: number;
    SenderID: string;
    Sender: UserProfileDataModel;
    ReceiverID: string;
    ReceiverName: string;
    Topic: string;
    Messages: Message[];
}