import { EventEmitter } from "@angular/core";

export enum ControlType {
  'input',
  'textarea'
  // select, option ...
}

export enum ButtonType {
  'button',
  'submit',
  'link'
}

export enum FieldType {
  'text',
  'number',
  'email',
  'password' //checkbox
}

export interface IFormControl {
  label: string;
  key: string;
  fieldType: FieldType;
  controlType: ControlType;
  required: boolean;
  visible: boolean;
}

export interface IButton {
  label: string;
  type: ButtonType;
  linkUrl?: string;
  alignEnd?: boolean;
  onClick?: (value: object) => void;
}

export interface IDynamicFormConfig {
  title: string;
  controls: IFormControl[];
  buttons: IButton[];
  data?: { [key: string]: any }
}
