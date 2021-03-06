import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../apiResponse';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ResetUserPassword } from '../models/user/resetUserPassword';
import { UpdateUserPasswordModel } from '../models/user/updateUserPassword.model';
import { UpdateUserProfileDataModel } from '../models/user/updateUserProfileData.model';
import { UserProfileDataModel } from '../models/user/userProfileData.model';
import { serverResetUserPasswordUrl, serverUserProfileDetailsUrl, serverUpdateUserProfileUrl, serverUpdateUserPasswordUrl, administrationGetAllUsers, administrationAddRoleToUserUrls, administrationDeleteUserUrls } from '../server-urls';
import { IdentityService } from './identity.service';
import { AddRoleModel } from '../models/administration/addRole.model';

@Injectable()
export class UserService {
    constructor(private httpClient: HttpClient, private identityService: IdentityService) { }

    getUserProfileData(): Observable<ApiResponse<UserProfileDataModel>> | null {
        if (!this.identityService.isSignedIn)
            return null;

        return this.httpClient.get<ApiResponse<UserProfileDataModel>>(serverUserProfileDetailsUrl);
    }
    updateUserProfile(model: UpdateUserProfileDataModel) {
        return this.httpClient.post(serverUpdateUserProfileUrl, model);
    }

    updatePassword(model: UpdateUserPasswordModel | ResetUserPassword) {
        if (model instanceof UpdateUserPasswordModel)
            return this.httpClient.post(serverUpdateUserPasswordUrl, model);

        return this.httpClient.post(serverResetUserPasswordUrl, model)
    }

    addRole(userId: string, role: string) {
        var data: AddRoleModel = {
            UserID: userId,
            Role: role
        }
        return this.httpClient.post(administrationAddRoleToUserUrls, data);
    }

    deleteUser(id: string) {
        return this.httpClient.delete(administrationDeleteUserUrls + id);
    }

    get isAdmin(): boolean {
        const helper = new JwtHelperService();
        const token = this.identityService.getToken;

        if (token == null || undefined)
            return false;

        const role = helper.decodeToken(token).role;

        if (role == "Administrator")
            return true;

        return false;
    }
}