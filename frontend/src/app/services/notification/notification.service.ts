import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NotificationData } from 'src/app/models/notification-data.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notification$: Subject<NotificationData> = new Subject();

  get notifications() {
    return this.notification$.asObservable();
  }

  constructor() { }

  public show(text: string, duration: number  = 3000) {
    this.notification$.next({text, duration});
  }
}
