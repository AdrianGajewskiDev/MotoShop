import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { TooltipPosition } from '@angular/material/tooltip';
import { ToastrService } from 'ngx-toastr';
import { buildProfileImagePath } from 'src/app/shared/Helpers/buildProfileImagePath';
import { slideInOutAnimation } from '../../shared/animations';
import { AllUsersModel } from '../../shared/models/administration/allUsers.model';
import { User } from '../../shared/models/administration/user.model';
import { AdministrationService } from '../../shared/services/administration.service';
import { UserDetailsDialogComponent } from '../user-details-dialog/user-details-dialog.component';

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
    private toastr: ToastrService, private dialog: MatDialog) { }

  //data
  private users: User[];

  public dataSource;

  //tabs
  usersTab: Element;
  productsTab: Element;
  servicesTab: Element;
  serverTab: Element;

  private tabs: { [key: string]: Element; } = {};

  ngOnInit(): void {
    this.usersTab = document.getElementById("users");
    this.productsTab = document.getElementById("products");
    this.servicesTab = document.getElementById("services");
    this.serverTab = document.getElementById("server");

    this.tabs =
    {
      "users": this.usersTab,
      "products": this.productsTab,
      "services": this.servicesTab,
      "server": this.serverTab,
    };
  }

  goToUserDetails(id) {

    const currentUser = this.users.filter(x => x.Id == id)[0];
    let data: User = new User();

    data.Email = currentUser.Email;
    data.EmailConfirmed = currentUser.EmailConfirmed;
    data.Id = currentUser.Id;
    data.ImageUrl = currentUser.ImageUrl;
    data.IsExternal = currentUser.IsExternal;
    data.LastName = currentUser.LastName;
    data.Name = currentUser.Name;
    data.UserName = currentUser.UserName;

    if (!data.IsExternal)
      data.ImageUrl = buildProfileImagePath(data.ImageUrl);

    console.log(data);

    this.dialog.open(UserDetailsDialogComponent, {
      minWidth: '900px',
      minHeight: '800px',
      direction: 'ltr',
      data: {
        User: data
      }
    });
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
