import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.sass']
})
export class LoadingSpinnerComponent implements OnInit {

  constructor() { }

  @Input() color: string = "#fff"
  ngOnInit(): void {
    document.getElementById("spinner")
  }

}
