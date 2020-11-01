import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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

  constructor(private fb: FormBuilder, private router:Router) { }

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
