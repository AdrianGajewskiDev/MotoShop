import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FormsMapper } from 'src/app/shared/mapper/formsMapper';
import { ResetUserPassword } from 'src/app/shared/models/user/resetUserPassword';
import { passwordValidators } from 'src/app/shared/password-validators';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.sass']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private mapper: FormsMapper,
    private service: UserService,
    private toastr: ToastrService) { }

  public form: FormGroup;
  public showError: boolean = false;
  public showLoadingSpinner: boolean = false;
  public message: string = "";

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      newPassword: ['', passwordValidators]
    });
  }

  onSubmit(): void {
    this.showLoadingSpinner = true;

    const model = this.mapper.map<ResetUserPassword>(new ResetUserPassword(), this.form);

    this.service.updatePassword(model).subscribe(
      () => {
        this.toastr.info(`We have sent message to the ${this.form.get('email').value} with confirmation link `)
      },
      error => {
        this.showLoadingSpinner = false;
        this.showError = true;
        this.message = error.error.message
      });
  }

}
