import { NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker'

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [BsDatepickerModule, NgIf,ReactiveFormsModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css'
})
export class DatePickerComponent implements ControlValueAccessor{
  label=input<string>("");
  maxDate=input<Date>();
  bsConfig?: Partial<BsDatepickerModule>;

  constructor(@Self() public ngControl:NgControl){
    this.ngControl.valueAccessor=this;
    this.bsConfig={
      containerClass:"theme-red",
      dateInputFormat:"YYYY MM DD"
    };
  }
  writeValue(obj: any): void {
    throw new Error('Method not implemented.');
  }
  registerOnChange(fn: any): void {
    throw new Error('Method not implemented.');
  }
  registerOnTouched(fn: any): void {
    throw new Error('Method not implemented.');
  }
  get control():FormControl{
    return this.ngControl.control as FormControl;
  }

}
