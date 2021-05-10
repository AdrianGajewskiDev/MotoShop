import { UserProfileDataModel } from "../user/userProfileData.model";
import { Message } from "./message.model";

export class Conversation {
    Id: number;
    SenderID: string;
    SenderName: string;
    ReceiverID: string;
    ReceiverName: string;
    Topic: string;
    Messages: Array<Message>;
}