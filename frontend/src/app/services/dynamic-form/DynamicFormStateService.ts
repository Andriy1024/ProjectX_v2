import { inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ButtonType, ControlType, IDynamicFormConfig, FieldType, IButton, IFormControl } from 'src/app/models/dynamic-form.model';
import { LoggerService } from '../logging/logger.service';

@Injectable({ providedIn: 'root' })
export class DynamicFormStateService {

    private readonly _logger = inject(LoggerService);
    private $configSubject = new BehaviorSubject<IDynamicFormConfig | null>(null);

    public config$ = this.$configSubject.asObservable();

    public get config(): IDynamicFormConfig | null {
        return this.$configSubject.getValue();
    }

    public push(config: IDynamicFormConfig): void {
        this.$configSubject.next(config);
    }

    private getDefaultConfig(): IDynamicFormConfig {
        const controls: IFormControl[] = [
            {
              label: 'Id',
              key: 'id',
              fieldType: FieldType.number,
              controlType: ControlType.input,
              required: false,
              visible: false,
            },
            {
              label: 'Name',
              key: 'name',
              fieldType: FieldType.text,
              controlType: ControlType.input,
              required: true,
              visible: true,
            },
            {
              label: 'Description',
              key: 'description',
              fieldType: FieldType.text,
              controlType: ControlType.textarea,
              required: false,
              visible: true,
            }
        ];

        const saveButton: IButton = {
            label: 'Save',
            type: ButtonType.submit,
            alignEnd: true,
            onClick: (value) => {
                this._logger.info('on save:', value);
            }
        };

        const deleteButton: IButton = {
            label: 'Delete',
            type: ButtonType.button,
            onClick: (value) => {
                this._logger.info('on delete', value);
            }
        };

        const cancelButton: IButton = {
            label: 'Cancel',
            type: ButtonType.link,
            linkUrl: '/'
        };

        return {
            title: 'Default form',
            controls: controls,
            buttons: [
                deleteButton,
                cancelButton,
                saveButton,
            ]
        };
    }
}
