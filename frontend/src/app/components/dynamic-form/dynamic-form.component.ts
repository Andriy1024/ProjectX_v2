import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ButtonType, ControlType, FieldType, IDynamicFormConfig, IFormControl } from 'src/app/models/dynamic-form.model';
import { DynamicFormStateService } from 'src/app/services/dynamic-form/DynamicFormStateService';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {

  public config: IDynamicFormConfig | null = null;
  public form: FormGroup = new FormGroup({});
  public controlTypes = ControlType;
  public fieldTypes = FieldType;
  public buttonTypes = ButtonType;

  public get visibleControls() {
    return this.config?.controls.filter(x => x.visible);
  }

  private configSubsription: Subscription;

  constructor(
    private readonly _stateService: DynamicFormStateService,
    private readonly _router: Router) {

      this.configSubsription = this._stateService.config$.subscribe(config => {
        if (config == null) {
          console.log('config empty');
          this._router.navigate(['/']);
        }

        console.log(config);
        this.config = config!;
        this.config.controls.forEach(x => {
          const control = new FormControl(this.config!.data?.[x.key] ?? null, this.getValidators(x));
          this.form.addControl(x.key, control);
        });
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.configSubsription.unsubscribe();
  }

  private getValidators(control: IFormControl): ValidatorFn[] {
    const validators: ValidatorFn[] = [];

    if (control.required) {
      validators.push(Validators.required);
    }
    if (control.fieldType == FieldType.email) {
      validators.push(Validators.email);
    }
    if (control.fieldType == FieldType.url) {
      const reg = '(https?://)([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';
      validators.push(Validators.pattern(reg));
    }

    return validators;
  }
}
