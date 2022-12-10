import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ButtonType, ControlType, IDynamicFormConfig, FieldType, IButton, IFormControl } from 'src/app/models/dynamic-form.model';

@Injectable({ providedIn: 'root' })
export class DynamicFormStateService {

    private $configSubject = new BehaviorSubject<IDynamicFormConfig | null>(this.getDefaultConfig());

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
                console.log('on save:');
                console.log(value);
            }
        };

        const deleteButton: IButton = {
            label: 'Delete',
            type: ButtonType.button,
            onClick: (value) => {
                console.log('on delete');
                console.log(value);
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
