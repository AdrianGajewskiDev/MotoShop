import { FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms'

export const passwordsMatchesValidator: ValidatorFn = (
  control: FormGroup
): ValidationErrors | null => {
  const password = control.get("password");
  const confirmPassword = control.get("confirmPassword");

  if (password.value != confirmPassword.value)
    return { passwordsMatchesValidator: true };
  else return null;
};