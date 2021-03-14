import { Injectable } from "@angular/core"
import { HttpClient } from "@angular/common/http"
import { serverRefreshTokenUrl, serverRegisterNewUserUrl, serverSignInUrl } from "../server-urls";
import { UserRegisterModel } from '../models/identity/userRegister.model';
import { SignInModel } from '../models/identity/signIn.model';
import { JwtHelperService } from "@auth0/angular-jwt";
import { RefreshTokenRequestModel } from "../models/identity/refreshTokenRequest.model";

@Injectable()
export class IdentityService {
    constructor(private httpClient: HttpClient) { }

    private registerUrl = serverRegisterNewUserUrl;
    private signedIn: boolean = localStorage.getItem("token") != null && localStorage.getItem("RefreshToken") != null;


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
    public saveRefreshToken(token: string) {
        localStorage.setItem("RefreshToken", token);
    }
    public get getRefreshToken(): string {
        return localStorage.getItem("RefreshToken");
    }
    public get tokenHasExpired(): boolean {
        const token = this.getToken;
        const helper = new JwtHelperService()
        return helper.isTokenExpired(token);
    }
    refreshToken() {
        const token = this.getToken;
        const refreshToken = this.getRefreshToken;

        let model: RefreshTokenRequestModel = {
            RefreshToken: refreshToken,
            Token: token
        }

        return this.httpClient.put(serverRefreshTokenUrl, model);
    }

    public get getUserID() {
        const helper = new JwtHelperService()

        return helper.decodeToken(this.getToken).UserID;
    }
}