import { HttpClient } from "@angular/common/http";
import { ThrowStmt } from "@angular/compiler";
import { Injectable } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { IHttpConnectionOptions } from "@aspnet/signalr";
import { ToastrService } from "ngx-toastr";
import { Subject } from "rxjs";
import { serverBaseUrl } from "../server-urls";
import { IdentityService } from "./identity.service";


@Injectable()
export class SignalRService {

    constructor(private service: IdentityService,
        private toastr: ToastrService) { }

    private hubConnection: signalR.HubConnection;

    private connectionStatus: "connected" | "disconnected" = "disconnected";

    public messageReceivedSubject = new Subject();

    public setConnectionStatus(value) {
        localStorage.setItem("connectionStatus", value);
    }

    public get getConnectionStatus() {
        return localStorage.getItem("connectionStatus");
    }

    public connected(): boolean {
        if (this.getConnectionStatus === "connected")
            return true;
        else
            return false;
    }

    public acquireConnection() {

        const options: IHttpConnectionOptions = {
            accessTokenFactory: () => {
                return this.service.getToken;
            }
        };

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(serverBaseUrl + "message", options)
            .build();

        this.hubConnection.onclose(() => {
            setTimeout(() => {
                this.connectionStatus = "disconnected";
                this.setConnectionStatus(this.connectionStatus);
                this.startConnection();
            }, 10);
        });
    }

    public startConnection() {
        this.hubConnection.start().then(() => {
            this.connectionStatus = "connected";
            this.setConnectionStatus(this.connectionStatus);
            console.log("Connected")
        }, error => console.log(error));

        this.hubConnection.on("message", (res) => this.messageReceivedSubject.next(res));
    }

    public listenFor(methodName: string, callback: (...args: any[]) => void) {
        this.hubConnection.on(methodName, callback);
    }
}