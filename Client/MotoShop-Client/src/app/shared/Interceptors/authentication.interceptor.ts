import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IdentityService } from '../services/identity.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor
{
    constructor(private service: IdentityService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if(!this.service.isSignedIn)
            return next.handle(req);

        let token = this.service.getToken;
        console.log(`intercepting: ${token}`);
        
        let clonedReq = req.clone({
        headers: req.headers.set(
          "Authorization",
          "Bearer " + token)
         });

        return next.handle(clonedReq);
    }
}
    