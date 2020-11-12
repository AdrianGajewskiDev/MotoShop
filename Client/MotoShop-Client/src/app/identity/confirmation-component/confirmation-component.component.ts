import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-confirmation-component',
  templateUrl: './confirmation-component.component.html',
  styleUrls: ['./confirmation-component.component.sass']
})
export class ConfirmationComponent implements OnInit {

  constructor(private activatedRoutes: ActivatedRoute) { }

  private confirmationType;
  public message: string = "";

  ngOnInit(): void {
    this.activatedRoutes.params.subscribe(
      (param) => {
        this.confirmationType = param
        switch (this.confirmationType) {
          case "email":
            this.message = "Your email has been successfully updated!!!";
            break;
          case "password":
            this.message = "Your password has been successfully changed!!!";
            break;
          case '':
            this.message = "Your email is now confirmed";
        }
      },
      () => this.message = "Ups, something went wrong"
    );
  }

}
