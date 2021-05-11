import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConversationService } from '../shared/services/conversation.service';
import { IdentityService } from '../shared/services/identity.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.sass']
})
export class NavbarComponent implements OnInit {

  constructor(private router: Router,
    private identityService: IdentityService,
    private convService: ConversationService) { }

  public isUserSignedIn: boolean = false;

  public unreadMessagesCount: number = 0;

  ngOnInit(): void {
    this.isUserSignedIn = this.identityService.isSignedIn;

    this.convService.getUnreadMessagesCount().subscribe((res: any) => {
      console.log(res.Count, "Count");
      this.unreadMessagesCount = res.Count
    }, error => console.log(error));
  }

  goTo(path: string): void {
    this.router.navigateByUrl(path);
  }
  logout(): void {
    this.identityService.logout();
    window.location.reload();
  }
}
