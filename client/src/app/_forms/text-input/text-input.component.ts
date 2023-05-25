import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';

  // why use self?
  // when we inject something into a constructor, it's going to check to see if it's
  // been used recently,
  // and if it has, it's going to reuse that thing that it's kept in memory
  // Now when it comes to our inputs, we do not want to reuse another control that was
  // alread in memory - we want to make sure that this ng control is unique to the
  // inputs that we're updating in the DOM 
  // the way we do that is by using the self decorator.
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
