import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { RefreshTokenRequestModel } from '../models/identity/refreshTokenRequest.model';
import { serverRefreshTokenUrl } from '../server-urls';
import { IdentityService } from '../services/identity.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
    constructor(private service: IdentityService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.service.isSignedIn || this.service.isRefreshTokenRequested == true)
            return next.handle(req);

        let token = this.service.getToken;

        if (!this.service.tokenHasExpired) {
            let clonedReq = req.clone({
                headers: req.headers.set(
                    "Authorization",
                    "Bearer " + token)
            });

            return next.handle(clonedReq);
        }
        this.service.isRefreshTokenRequested = true;

        let requestBody: RefreshTokenRequestModel = {
            RefreshToken: this.service.getRefreshToken,
            Token: this.service.getToken
        }

        let refreshTokenRequest: HttpRequest<any> = new HttpRequest<any>("PUT", serverRefreshTokenUrl, requestBody);

        next.handle(refreshTokenRequest).toPromise().then((res: any) => {
            this.service.saveToken(res.body.Token);
            this.service.saveRefreshToken(res.body.RefreshToken)
            this.service.isRefreshTokenRequested = false;
        },
            error => {
                console.log(error);
            });

        this.router.navigateByUrl("/home");
    }
}
