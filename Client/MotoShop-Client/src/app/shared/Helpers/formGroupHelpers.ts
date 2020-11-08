import { FormGroup } from '@angular/forms';

export function isEmpty(formGroup: FormGroup): boolean {
    let result = true;
    let controls = formGroup.controls;

    Object.keys(controls).forEach(key => {
        if (controls[key].value != null)
            result = false
    });

    return result;
}