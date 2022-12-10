import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ButtonType, ControlType, IDynamicFormConfig, FieldType, IButton, IFormControl } from 'src/app/models/dynamic-form.model';

@Injectable({ providedIn: 'root' })
export class DynamicFormStateService {

    private $config = new BehaviorSubject<IDynamicFormConfig | null>(this.getDefaultConfig());

    public push(config: IDynamicFormConfig): void {
        this.$config.next(config);
    }

    public getConfig() {
        return this.$config.asObservable();
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
            onClick: new EventEmitter<object>()
        };

        saveButton.onClick!.subscribe((value) => {
            console.log('on save:');
            console.log(value);
        });

        const deleteButton: IButton = {
            label: 'Delete',
            type: ButtonType.button,
            onClick: new EventEmitter<object>()
        };

        deleteButton.onClick!.subscribe((value) => {
            console.log('on delete');
            console.log(value);
        });

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
