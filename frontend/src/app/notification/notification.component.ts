import { animate, style, transition, trigger } from '@angular/animations';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NotificationService } from '../services/notification/notification.service';

@Component({
    selector: 'app-notification',
    templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.scss'],
    standalone: true,
    imports: [CommonModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
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
    ]
})
export class NotificationComponent {

  public readonly notification = signal<string | null>(null);

  private timeoutId: ReturnType<typeof setTimeout> | undefined;
  private readonly _notificationService = inject(NotificationService);

  constructor() {
    this._notificationService.notifications
      .pipe(takeUntilDestroyed())
      .subscribe((notification) => {
        this.notification.set(notification.text);
        
        if (this.timeoutId) {
          clearTimeout(this.timeoutId);
        }
        
        this.timeoutId = setTimeout(() => { 
          this.notification.set(null); 
        }, notification.duration);
      });
  }
}
