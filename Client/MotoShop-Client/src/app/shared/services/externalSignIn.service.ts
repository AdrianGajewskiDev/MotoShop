import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { copyFileSync } from "fs";
import { GoogleAuthService } from "ng-gapi";
import { ExternalSignInModel } from "../models/identity/externalSignIn.model";
import { serverExternalSignInUrl } from "../server-urls";


@Injectable()
export class ExternalSignInService {
    constructor(private service: GoogleAuthService, private client: HttpClient) { }

    signIn(): void {
        this.service.getAuth().subscribe(res => {
            res.signIn().then(user => this.externalSignInCallback(user))
        });
    }

    externalSignInCallback(user: gapi.auth2.GoogleUser) {

        let model: ExternalSignInModel =
        {
            AccessToken: user.getAuthResponse().id_token,
            Provider: "Google"
        }

        console.log(model.AccessToken);


        this.client.post(serverExternalSignInUrl, model).subscribe(
            res => {
                console.log(res)
            },
            error => console.log(error)

        )
    }
}