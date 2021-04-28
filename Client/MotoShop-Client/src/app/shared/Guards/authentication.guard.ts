import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { IdentityService } from '../services/identity.service';


@Injectable({ providedIn: "root" })
export class AuthenticationGuard implements CanActivate {
    constructor(private service: IdentityService,
        private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        console.log(this.getResolvedUrl(route));


        if (this.getResolvedUrl(route) == "/identity" && this.service.isSignedIn) {
            this.router.navigateByUrl('/home');
            return false;
        }

        if (this.service.isSignedIn)
            return true;

        this.router.navigateByUrl('/identity');
        return;
    }
    getResolvedUrl(route: ActivatedRouteSnapshot): string {
        return route.pathFromRoot
            .map(v => v.url.map(segment => segment.toString()).join('/'))
            .join('/');
    }

}