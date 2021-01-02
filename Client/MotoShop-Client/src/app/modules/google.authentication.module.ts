import { NgModule } from "@angular/core";
import { NgGapiClientConfig, GoogleApiModule, NG_GAPI_CONFIG } from "ng-gapi/ng-gapi"

const googleAPIConfig: NgGapiClientConfig = {
    discoveryDocs: ["https://analyticsreporting.googleapis.com/$discovery/rest?version=v4"],
    client_id: "78220292618-ean89urhcnt15u383q98ggke9sasuofv.apps.googleusercontent.com"
}

@NgModule({
    imports: [
        GoogleApiModule.forRoot(
            {
                provide: NG_GAPI_CONFIG,
                useValue: googleAPIConfig
            })
    ]
})
export class GoogleAuthenticationModule { }