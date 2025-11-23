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
    changeDetection: ChangeDetectionStrategy.OnPush
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
