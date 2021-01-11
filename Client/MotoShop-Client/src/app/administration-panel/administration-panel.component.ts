import { Component, OnInit } from '@angular/core';
import { slideInOutAnimation } from '../shared/animations';

@Component({
  selector: 'app-administration-panel',
  templateUrl: './administration-panel.component.html',
  styleUrls: ["administration.component.sass"],
  animations: [slideInOutAnimation]
})
export class AdministrationPanelComponent implements OnInit {

  public animationState: "slideIn" | "slideOut" = "slideIn";

  constructor() { }

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
      "server": this.serverTab
    };
  }

  switchTabs(tab): void {
    this.tabs[tab].classList.add('show');

    Object.keys(this.tabs).forEach(element => {
      if (element != tab)
        this.tabs[element].classList.remove('show');
    });
  }

}
