import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { GoogleAuthService } from "ng-gapi";
import { BehaviorSubject } from "rxjs";
import { ExternalSignInModel } from "../models/identity/externalSignIn.model";
import { serverExternalSignInUrl } from "../server-urls";
import { IdentityService } from "./identity.service";


@Injectable()
export class ExternalSignInService {
    constructor(private service: GoogleAuthService,
        private client: HttpClient,
        private router: Router,
        private identityService: IdentityService) { }


    googleSignIn() {
        this.service.getAuth().subscribe(res => {
            res.signIn().then(user => this.externalSignInCallback(user))

        });
    }
    externalSignInCallback(user: gapi.auth2.GoogleUser) {
        let model: ExternalSignInModel =
        {
            AccessToken: user.getAuthResponse().id_token,
            Provider: "Google",
            ProviderID: "google.com"
        }

        this.client.post(serverExternalSignInUrl, model)
            .subscribe((res: any) => {
                this.identityService.saveToken(res.Token);
                this.router.navigateByUrl("/home").then(() => {
                    window.location.reload();
                });
            },
                error => console.log(error)
            );

    }
}