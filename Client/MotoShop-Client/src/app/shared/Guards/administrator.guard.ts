import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { UserService } from '../services/user.service';


@Injectable({ providedIn: "root" })
export class AdministratorGuard implements CanActivate {
    constructor(private service: UserService,
        private toastr: ToastrService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        // const result = this.service.isAdmin;

        // if (result == true)
        //     return true;

        // this.toastr.error("You don't have permissions to access this page")
        // return false;

        return true;
    }

}