import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { ToastrService } from "ngx-toastr";
import { serverBaseUrl } from "../server-urls";


@Injectable()
export class SignalRService {

    constructor(private client: HttpClient, private toastr: ToastrService) { }

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
        this.hubConnection.on("message", (res) => this.toastr.info(res));
    }

    public listenFor(methodName: string, callback: (...args: any[]) => void) {
        this.hubConnection.on(methodName, callback);
    }

    test() {
        return this.client.get(serverBaseUrl + "api/messages");
    }
}