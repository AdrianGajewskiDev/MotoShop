import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { slideInOutAnimation } from 'src/app/shared/animations';
import { passwordsMatchesValidator } from 'src/app/shared/custom-validators';
import { passwordValidators} from "../../shared/password-validators"

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass'],
  animations: [slideInOutAnimation]
})
export class RegisterComponent implements OnInit {

  constructor(private fb:FormBuilder) { }

  public animationState: "slideIn" | "slideOut" = "slideIn";
  public registerForm:FormGroup;

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        username: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', passwordValidators],
        confirmPassword: ['', [Validators.required]]
      },
      {
        validators: passwordsMatchesValidator
      });
  }
  onSubmit():void{
  }
}
