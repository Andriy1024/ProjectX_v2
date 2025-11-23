import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DynamicFormComponent } from './dynamic-form.component';

@NgModule({
  declarations: [DynamicFormComponent],
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  exports: [DynamicFormComponent]
})
export class DynamicFormModule { }
