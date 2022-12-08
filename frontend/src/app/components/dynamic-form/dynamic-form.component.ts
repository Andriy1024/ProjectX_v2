import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {

  public controls: IFormControl[] = [];
  public form: FormGroup = new FormGroup({});
  public data: any = {};
  public controlType = ControlType;

  constructor() { }

  ngOnInit(): void {
    this.controls = [
      {
        label: 'Name',
        key: 'name',
        fieldType: FieldType.text,
        controlType: this.controlType.input,
        required: true
      },
      {
        label: 'Description',
        key: 'description',
        fieldType: FieldType.text,
        controlType: this.controlType.textarea,
        required: false
      }
    ];

    this.controls.forEach(x => {
      const control =  new FormControl(this.data[x.key], this.getValidators(x));

      this.form.addControl(x.key, control);
    });
  }

  public onSubmit() {
    console.log(this.form.value);
  }

  private getValidators(control :IFormControl): ValidatorFn[] {
    const validators: ValidatorFn[] = [];

    if(control.required) {
      validators.push(Validators.required);
    }

    if (control.fieldType == FieldType.email) {
      validators.push(Validators.email);
    }

    return validators;
  }

}


export interface IFormControl 
{
  label: string;
  key: string;
  fieldType: FieldType;
  controlType: ControlType;
  required: boolean;
}


export enum FieldType {
  'text', 
  'number', 
  'email', 
  'password' //checkbox
} 

export enum ControlType { 
  'input', 
  'textarea'
  // select, option ... 
}