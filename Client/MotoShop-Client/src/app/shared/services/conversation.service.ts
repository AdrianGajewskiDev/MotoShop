import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { serverGetConversationUrl } from "../server-urls";

@Injectable()
export class ConversationService {
    constructor(private client: HttpClient) { }

    getConversation(senderID, recipientID, topic) {
        return this.client.get(serverGetConversationUrl + `?senderID=${senderID}&recipientID=${recipientID}&topic=${topic}`)
    }
}