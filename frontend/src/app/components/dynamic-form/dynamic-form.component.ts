import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ButtonType, ControlType, FieldType, IDynamicFormConfig, IFormControl } from 'src/app/models/dynamic-form.model';
import { DynamicFormStateService } from 'src/app/services/dynamic-form/DynamicFormStateService';
import { LoggerService } from 'src/app/services/logging/logger.service';

@Component({
    selector: 'app-dynamic-form',
    templateUrl: './dynamic-form.component.html',
    styleUrls: ['./dynamic-form.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterModule],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class DynamicFormComponent implements OnInit {

  public config: IDynamicFormConfig | null = null;
  public form: FormGroup | undefined;
  public controlTypes = ControlType;
  public fieldTypes = FieldType;
  public buttonTypes = ButtonType;

  private readonly _stateService = inject(DynamicFormStateService);
  private readonly _router = inject(Router);
  private readonly _logger = inject(LoggerService);
  private readonly _destroyRef = inject(DestroyRef);

  public get visibleControls() {
    return this.config?.controls.filter(x => x.visible);
  }

  constructor() {
    this._stateService.config$
      .pipe(takeUntilDestroyed())
      .subscribe(config => {
          if (config == null) {
            this._logger.warn('Dynamic form config is empty, redirecting to root');
            this._router.navigate(['/']);
            return;
          }

          this.config = config;
          this.form = new FormGroup({});
          this.config.controls.forEach(x => {
            const control = new FormControl(this.config!.data?.[x.key] ?? null, this.getValidators(x));
            this.form!.addControl(x.key, control);
          });
        }
      );
  }

  ngOnInit(): void {
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
