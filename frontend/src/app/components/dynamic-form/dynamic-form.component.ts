import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ButtonType, ControlType, FieldType, IButton, IDynamicFormConfig, IFormControl } from 'src/app/models/dynamic-form.model';
import { DynamicFormStateService } from 'src/app/services/dynamic-form/DynamicFormStateService';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {

  public get visibleControls() {
    return this.config?.controls.filter(x => x.visible);
  }

  public config: IDynamicFormConfig | undefined;

  public form: FormGroup = new FormGroup({});
  public data: any = {};

  public controlTypes = ControlType;
  public buttonTypes = ButtonType;

  constructor(
    private readonly stateService: DynamicFormStateService) {

      this.stateService.getConfig().subscribe(config => {
        if (config == null) {
          console.log('config empty');
          return;
        }

        console.log(config);

        this.config = config;

        this.config.controls.forEach(x => {
          const control =  new FormControl(this.data[x.key], this.getValidators(x));

          this.form.addControl(x.key, control);
        });

      });
  }

  ngOnInit(): void {
  }

  private getValidators(control: IFormControl): ValidatorFn[] {
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
