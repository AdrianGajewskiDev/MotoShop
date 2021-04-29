import { Injectable } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { serverBaseUrl } from "../server-urls";


@Injectable()
export class SignalRService {

    private hubConnection: signalR.HubConnection;

    public startConnection() {
        this.hubConnection = new signalR.HubConnectionBuilder().withUrl(serverBaseUrl + "message").build();


        this.hubConnection.start().then(() => console.log("Connected"), error => console.log(error));

        this.hubConnection.on("test", (res) => console.log(res));
    }
}