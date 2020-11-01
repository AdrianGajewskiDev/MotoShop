import { Validators } from '@angular/forms';

export const passwordValidators = 
[
    Validators.required,
    Validators.minLength(7),
    Validators.maxLength(20)
];