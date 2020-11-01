import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-identity-placeholder',
  templateUrl: './identity-placeholder.component.html',
  styleUrls: ['./identity-placeholder.component.sass']
})
export class IdentityPlaceholderComponent implements OnInit {

  private switcherPosition: "left" | "right" = "left";

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    document.getElementById("left-button").classList.add("active")
  }

  async switchbtn(btn:string){
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
          this.router.navigate([{ outlets: {  identity: ['login'] } }] ,{relativeTo:this.activatedRoute})
        }
        break;
      case "right":
        {
          document.getElementById("right-button").classList.toggle("active");
          document.getElementById("left-button").classList.toggle("active");
          this.router.navigate([{ outlets: {  identity: ['register'] } }] ,{relativeTo:this.activatedRoute})
        }
        break;
      default:
        break;
    }
  }
}
