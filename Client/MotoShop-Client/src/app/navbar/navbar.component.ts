import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IdentityService } from '../shared/services/identity.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.sass']
})
export class NavbarComponent implements OnInit {

  constructor(private router:Router,
     private identityService: IdentityService) { }

  public isUserSignedIn: boolean = false;

  ngOnInit(): void {
    this.isUserSignedIn = this.identityService.isSignedIn;
  }

  goTo(path:string):void{
    this.router.navigateByUrl(path);
  }
logout():void{
  this.identityService.logout();
  window.location.reload();
}
}
