import { UserProfileDataModel } from "../user/userProfileData.model";
import { Message } from "./message.model";

export class Conversation {
    Id: number;
    SenderID: string;
    Sender: UserProfileDataModel;
    ReceiverID: string;
    Receiver: UserProfileDataModel;
    Topic: string;
    Messages: Message[];
}