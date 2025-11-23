import { animate, style, transition, trigger } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { scan } from 'rxjs';
import { NotificationService } from '../services/notification/notification.service';

@Component({
    selector: 'app-notification',
    templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.scss'],
    animations: [
        trigger('notificationAnim', [
            transition(':enter', [
                style({
                    opacity: 0,
                    transform: 'translateY(5px)'
                }),
                animate(250)
            ]),
            transition(':leave', [
                animate(125, style({
                    opacity: 0,
                    transfrom: 'scale(0.85)'
                }))
            ])
        ])
    ],
    standalone: false
})
export class NotificationComponent implements OnInit {

  public notification: string | null = null;

  private timeout: any;

  constructor(private readonly _notificationService: NotificationService) { }

  ngOnInit(): void {
    this._notificationService.notifications
      .subscribe((notification) => {
        this.notification = notification.text;
        clearTimeout(this.timeout);
        setTimeout(() => { this.notification = null; }, notification.duration);
      });
  }
}
