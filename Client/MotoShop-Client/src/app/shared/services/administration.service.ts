import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AllUsersModel } from "../models/administration/allUsers.model";
import { administrationGetAllUsers } from "../server-urls";

@Injectable()
export class AdministrationService {

    constructor(private client: HttpClient) { }

    getAllUsers(): Observable<AllUsersModel> {
        return this.client.get<AllUsersModel>(administrationGetAllUsers);
    }
}