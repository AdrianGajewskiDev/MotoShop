import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TooltipPosition } from '@angular/material/tooltip';
import { ToastrService } from 'ngx-toastr';
import { slideInOutAnimation } from '../shared/animations';
import { AllUsersModel } from '../shared/models/administration/allUsers.model';
import { User } from '../shared/models/administration/user.model';
import { AdministrationService } from '../shared/services/administration.service';

@Component({
  selector: 'app-administration-panel',
  templateUrl: './administration-panel.component.html',
  styleUrls: ["administration.component.sass"],
  animations: [slideInOutAnimation],
  encapsulation: ViewEncapsulation.None,
})
export class AdministrationPanelComponent implements OnInit {

  public animationState: "slideIn" | "slideOut" = "slideIn";
  public tooltipPosition: TooltipPosition = 'right';
  public displayedColumns: string[] = ['Id', 'Username', 'Name', 'Email'];

  constructor(private service: AdministrationService,
    private toastr: ToastrService) { }

  //data
  private users: User[];

  public currentSelectedUser: User;
  public dataSource;

  //tabs
  usersTab: Element;
  productsTab: Element;
  servicesTab: Element;
  serverTab: Element;
  userDetailsTab: Element;

  private tabs: { [key: string]: Element; } = {};

  ngOnInit(): void {
    this.usersTab = document.getElementById("users");
    this.productsTab = document.getElementById("products");
    this.servicesTab = document.getElementById("services");
    this.serverTab = document.getElementById("server");
    this.userDetailsTab = document.getElementById("userDetails");

    this.tabs =
    {
      "users": this.usersTab,
      "products": this.productsTab,
      "services": this.servicesTab,
      "server": this.serverTab,
      "userDetails": this.userDetailsTab
    };
  }

  setCurrentUser(id) {
    this.currentSelectedUser = this.users.filter(x => x.Id == id)[0];

    console.log(this.currentSelectedUser);
  }

  switchTabs(tab, details: boolean): void {
    this.tabs[tab].classList.add('show');

    Object.keys(this.tabs).forEach(element => {
      if (element != tab)
        this.tabs[element].classList.remove('show');
    });

    if (!details)
      this.getData(tab);
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  getData(tab: string) {

    switch (tab) {
      case 'users':
        {
          if (this.users == null)
            this.service.getAllUsers().subscribe(
              (res: AllUsersModel) => {
                this.users = res.Users
                this.dataSource = new MatTableDataSource(this.users);
              },
              error => {
                this.toastr.error(error.message);
              }
            );
        }
        break;
      case 'products':
        {

        }
        break;
      case 'services':
        {

        }
        break;
      case 'server':
        {

        }
        break;
    }
  }

}
