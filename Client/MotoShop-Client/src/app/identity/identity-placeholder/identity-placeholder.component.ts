import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-identity-placeholder',
  templateUrl: './identity-placeholder.component.html',
  styleUrls: ['./identity-placeholder.component.sass']
})
export class IdentityPlaceholderComponent implements OnInit {

  private switcherPosition: "left" | "right" = "left";

  constructor() { }

  ngOnInit(): void {
    document.getElementById("left-button").classList.add("active")
  }

  switchbtn(btn:string) :void{
    if(this.switcherPosition == btn)
      return;

    if(btn === "left")
      this.switcherPosition = "left";
    else
      this.switcherPosition = "right"

    switch (this.switcherPosition) {
      case "left":
        {
          document.getElementById("left-button").classList.toggle("active");
          document.getElementById("right-button").classList.toggle("active");
        }
        break;
      case "right":
        {
          document.getElementById("right-button").classList.toggle("active");
          document.getElementById("left-button").classList.toggle("active");
        }
        break;
      default:
        break;
    }
  }
}
