import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

export function MatchValidator(matchingControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control.parent as FormGroup;
    if (!formGroup) {
      return null;
    }

    const matchingControlValue = formGroup.controls[matchingControlName].value;

    if (control.value !== matchingControlValue) {
      return { mismatch: true };
    } else {
      return null;
    }
  };
}
