import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NewMessageModel } from "../models/messages/newMessage.model";
import { serverGetConversationUrl, serverSendMessageUrl } from "../server-urls";

@Injectable()
export class ConversationService {
    constructor(private client: HttpClient) { }

    getConversation(senderID, recipientID, topic) {
        return this.client.get(serverGetConversationUrl + `?senderID=${senderID}&recipientID=${recipientID}&topic=${topic}`)
    }

    sendMessage(model: NewMessageModel) {
        return this.client.post(serverSendMessageUrl, model);
    }
}