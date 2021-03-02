import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { TooltipPosition } from '@angular/material/tooltip';
import { ToastrService } from 'ngx-toastr';
import { buildImagePath } from 'src/app/shared/Helpers/buildProfileImagePath';
import { slideInOutAnimation } from '../../shared/animations';
import { AllUsersModel } from '../../shared/models/administration/allUsers.model';
import { User } from '../../shared/models/administration/user.model';
import { AdministrationService } from '../../shared/services/administration.service';
import { UserDetailsDialogComponent } from '../../Dialogs/user-details-dialog/user-details-dialog.component';
import { ConfirmationComponent } from 'src/app/identity/confirmation-component/confirmation-component.component';
import { ConfirmationDialogComponent } from 'src/app/Dialogs/confirmation-dialog/confirmation-dialog.component';
import { Advertisement } from 'src/app/shared/models/advertisements/advertisement.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { AdvertisementDetailsDialogComponent } from 'src/app/Dialogs/advertisement-details-dialog/advertisement-details-dialog.component';

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
    private advertisementService: AdvertisementsService,
    private toastr: ToastrService, private dialog: MatDialog) { }

  //data
  private users: User[];
  private advertisements;
  public currentAdvertisements: Advertisement[] = [];

  public dataSource;
  public showLoadingSpinner: boolean = false;

  //tabs
  usersTab: Element;
  productsTab: Element;
  servicesTab: Element;
  serverTab: Element;

  //products paginator data
  public pageNumber = 1;
  public perPage = 15;
  public totalPages;

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
      data.ImageUrl = buildImagePath(data.ImageUrl);

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
    this.showLoadingSpinner = false;
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
    this.showLoadingSpinner = true;
    switch (tab) {
      case 'users':
        {
          if (this.users != null)
            this.showLoadingSpinner = false;
          else
            this.service.getAllUsers().subscribe(
              (res: AllUsersModel) => {
                this.showLoadingSpinner = false;
                this.users = res.Users
                this.dataSource = new MatTableDataSource(this.users);
              },
              error => {
                this.showLoadingSpinner = false;
                this.toastr.error(error.message);
              }
            );
        }
        break;
      case 'products':
        {
          if (this.advertisements != null)
            this.showLoadingSpinner = false;
          else
            this.getAdvertisements(this.perPage, this.pageNumber);
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
  seedDatabase() {
    this.dialog.open(ConfirmationDialogComponent, {
      width: '250px',
      data:
      {
        func: (s) => {
          if (s == false) {
            this.seedDatabaseCallback()
          }
        },
        run: false,
        message: "Are you sure you want to add items to database? This action cannot be undo"
      }
    })
  }
  seedDatabaseCallback() {
    this.showLoadingSpinner = true;
    this.service.seedDb().subscribe(() => {
      this.showLoadingSpinner = false;
      this.toastr.info("Successfully added items to database");
    },
      error => {
        this.showLoadingSpinner = false;
        this.toastr.error(error)
      });
  }
  setCurrentAdvertisementToShow(itemType: string): Advertisement[] | null {
    if (!this.advertisements)
      return null;

    if (itemType == "All")
      return this.advertisements;
    else
      return this.advertisements.filter(x => x.ShopItem.ItemType == itemType);
  }
  nextPage() {
    if (this.pageNumber >= this.totalPages)
      return;
    else {
      this.pageNumber += 1;
      console.log(this.pageNumber);
      this.getAdvertisements(this.perPage, this.pageNumber);
      console.log();
    }
  }
  previousPage() {
    if (this.pageNumber == 1)
      return;
    else {
      this.pageNumber -= 1;
      this.getAdvertisements(this.perPage, this.pageNumber);
    }
  }
  getAdvertisements(perPage: number, pageNumber: number) {
    this.advertisementService.getAll(perPage, pageNumber).subscribe(
      (res: any) => {
        +
          console.log(res);

        this.advertisements = res.Content;
        this.showLoadingSpinner = false;
        this.currentAdvertisements = this.setCurrentAdvertisementToShow("All");
        this.totalPages = res.TotalPages;
      },
      error => {
        this.showLoadingSpinner = false;
        this.toastr.error(error.message);
      });
  }
  goToDetails(id) {
    this.dialog.open(AdvertisementDetailsDialogComponent, {
      width: '1000px',
      height: '600px',
      data: {
        AdvertisementID: id
      }
    });
  }
}
