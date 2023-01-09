import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthRequest } from 'src/app/auth/auth.models';
import { AuthService } from 'src/app/auth/services/auth-service.service';
import { ButtonType, ControlType, FieldType } from 'src/app/models/dynamic-form.model';
import { DynamicFormStateService } from 'src/app/services/dynamic-form/DynamicFormStateService';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  private _returnUrl!: string;

  constructor(
    private readonly _stateService: DynamicFormStateService,
    private readonly _authService: AuthService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router)
  {
  }

  ngOnInit(): void {
    this._returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
    this.pushFormConfig();
  }

  private pushFormConfig() {
    this._stateService.push({
      title: 'Sign in',
      controls: [
        {
          label: 'Email',
          key: 'email',
          fieldType: FieldType.email,
          controlType: ControlType.input,
          required: true,
          visible: true,
        },
        {
          label: 'Password',
          key: 'password',
          fieldType: FieldType.password,
          controlType: ControlType.input,
          required: true,
          visible: true,
        },
      ],
      buttons: [
        {
          label: 'Sign In',
          type: ButtonType.submit,
          onClick: this.onSignIn
        }
      ],
      data: undefined
    });
  }

  public onSignIn = (value: object): void => {
    this._authService.logIn(value as AuthRequest)
      .subscribe(r => {
        this._router.navigateByUrl(this._returnUrl);
      });
  }
}
