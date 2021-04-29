import { Injectable } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { serverBaseUrl } from "../server-urls";


@Injectable()
export class SignalRService {

    private hubConnection: signalR.HubConnection;

    public acquireConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(serverBaseUrl + "message")
            .build();

        this.hubConnection.onclose(() => {
            setTimeout(() => {
                this.startConnection();
            }, 10);
        });
    }

    public startConnection() {
        this.hubConnection.start().then(() => console.log("Connected"), error => console.log(error));
    }

    public listenFor(methodName: string, callback: (...args: any[]) => void) {
        this.hubConnection.on(methodName, callback);
    }

}