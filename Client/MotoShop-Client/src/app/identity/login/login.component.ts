import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  public loginForm: FormGroup;

  ngOnInit(): void {
    this.loginForm = this.fb.group(
      {
        data: new FormControl('', Validators.required),
        password: ['', Validators.required]
      });
  }

}
