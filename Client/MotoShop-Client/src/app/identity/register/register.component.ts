import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { slideInOutAnimation } from 'src/app/shared/animations';
import { passwordsMatchesValidator } from 'src/app/shared/custom-validators';
import { FormsMapper } from 'src/app/shared/mapper/formsMapper';
import { UserRegisterModel } from 'src/app/shared/models/user/userRegister.model';
import { IdentityService } from 'src/app/shared/services/identity.service';
import { passwordValidators} from "../../shared/password-validators"
import { ToastrService } from "node_modules/ngx-toastr"

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass'],
  animations: [slideInOutAnimation]
})
export class RegisterComponent implements OnInit {

  constructor(private fb:FormBuilder,
     private service: IdentityService,
     private mapper: FormsMapper,
     private toastr: ToastrService
    ) { }

  public animationState: "slideIn" | "slideOut" = "slideIn";
  public registerForm:FormGroup;

  ngOnInit(): void {

    this.registerForm = this.fb.group(
      {
        username: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        name: ['', Validators.required],
        lastName: ['', Validators.required],
        password: ['', passwordValidators],
        confirmPassword: ['', [Validators.required]]
      },
      {
        validators: passwordsMatchesValidator
      });
  }
  onSubmit():void{
    let model = this.mapper.map<UserRegisterModel>(new UserRegisterModel(), this.registerForm);
    this.service.registerUser(model).subscribe(
      (res) => {
        this.toastr.info("Your account has been created successfully. You can now sign in");
      },
      (error) => console.log(error)
      );
  }
}
