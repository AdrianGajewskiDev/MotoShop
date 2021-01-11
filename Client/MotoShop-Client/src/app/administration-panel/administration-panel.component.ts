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

  ngOnInit(): void {
  }

}
