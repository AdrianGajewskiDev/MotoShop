import { Injectable } from "@angular/core"
import { HttpClient} from "@angular/common/http"
import { serverRegisterNewUserUrl} from "../server-urls";
import { UserRegisterModel } from '../models/user/userRegister.model';

@Injectable()
export class IdentityService{
    constructor(private httpClient: HttpClient) { }

    private registerUrl = serverRegisterNewUserUrl;

    registerUser(model: UserRegisterModel) {
        return this.httpClient.post<UserRegisterModel>(this.registerUrl, model);
    }

}