import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { IdentityService } from '../services/identity.service';


@Injectable()
export class AuthenticationGuard implements CanActivate {
    constructor(private service: IdentityService,
        private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        if (this.service.isSignedIn)
            return true;

        return this.router.navigateByUrl("/identity");
    }
}