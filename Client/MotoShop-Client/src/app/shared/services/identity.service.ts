import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { serverRegisterNewUserUrl, serverSignInUrl } from "../server-urls";
import { UserRegisterModel } from '../models/identity/userRegister.model';
import { SignInModel } from '../models/identity/signIn.model';
import { BehaviorSubject } from "rxjs";

@Injectable()
export class IdentityService {
    constructor(private httpClient: HttpClient) { }

    private registerUrl = serverRegisterNewUserUrl;
    private signedIn: boolean = localStorage.getItem("token") != null;


    registerUser(model: UserRegisterModel) {
        return this.httpClient.post<UserRegisterModel>(this.registerUrl, model);
    }

    signIn(signInCredentials: SignInModel) {
        return this.httpClient.post(serverSignInUrl, signInCredentials);
    }

    logout(): void {
        if (this.isSignedIn)
            this.removeToken();
    }

    public saveToken(token: string): void {
        localStorage.setItem("token", token);
    }

    public removeToken() {
        localStorage.removeItem("token");
    }
    public get getToken(): string {
        let token = localStorage.getItem("token")

        if (token != null)
            return token;

        return "";
    }


    public get isSignedIn(): boolean {
        return this.signedIn;
    }


}