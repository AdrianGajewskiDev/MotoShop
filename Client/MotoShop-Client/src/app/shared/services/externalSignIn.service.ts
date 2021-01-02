import { Injectable } from "@angular/core";
import { GoogleAuthService } from "ng-gapi";


@Injectable()
export class ExternalSignInService {
    constructor(private service: GoogleAuthService) { }

    signIn(): void {
        this.service.getAuth().subscribe(res => {
            res.signIn().then(user => this.externalSignInCallback(user))
        });
    }

    externalSignInCallback(user) {
        console.log(user);

    }
}