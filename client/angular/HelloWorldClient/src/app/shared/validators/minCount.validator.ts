import { ValidatorFn, AbstractControl, FormArray } from '@angular/forms';

export function minCount(count: number): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const formArray = control as FormArray;

    if (formArray && formArray.length < count) {
      return { minCount: true };
    }

    return null;
  };
}
