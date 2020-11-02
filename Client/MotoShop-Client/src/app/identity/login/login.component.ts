import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { slideInOutAnimation } from 'src/app/shared/animations';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'],
  encapsulation: ViewEncapsulation.None,
  animations: [
  slideInOutAnimation
  ]
})
export class LoginComponent implements OnInit{

/*
TODO:
  -add user service to handle login method
  -implement onSubmit()
 */

  constructor(private fb: FormBuilder) { }

  public loginForm: FormGroup;
  public animationState: "slideIn" | "slideOut" = "slideIn";

   ngOnInit() :void{
    this.loginForm = this.fb.group(
      {
        data: new FormControl('', Validators.required),
        password: ['', Validators.required]
      });
  }

  onSubmit():void{
    
  }


}
