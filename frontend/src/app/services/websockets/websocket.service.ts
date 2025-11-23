import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import * as Rx from 'rxjs';
import { map, switchMap, tap } from 'rxjs';
import { AnonymousSubject } from 'rxjs/internal/Subject';
import { REALTIME_API_URL } from 'src/app/app-injection-tokens';
import { IRealtimeMessage } from 'src/app/models/realtime.model';
import { LoggerService } from '../logging/logger.service';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  private readonly _http = inject(HttpClient);
  private readonly realtimeUrl = inject(REALTIME_API_URL);
  private readonly _logger = inject(LoggerService);

  private subject: Rx.Subject<MessageEvent> | null = null;

  public subscribe(): Rx.Observable<IRealtimeMessage<unknown>> {
    if (this.subject && !this.subject.closed) {
      return this.subject.pipe(
        tap(msg => this._logger.debug('WS Message', msg)),
        map(r => JSON.parse(r.data) as IRealtimeMessage<unknown>)
      );
    }

    return this._http
      .post<{connectionId: string}>(`${this.realtimeUrl}/api/realtime/connect`, {})
        .pipe(
          switchMap((response: { connectionId: string }) => {
            this.subject = this.createWS(response.connectionId);
            return this.subject.pipe(
              map(r => JSON.parse(r.data) as IRealtimeMessage<unknown>)
            );
          })
        );
  }

  private createWS(connectionId: string): AnonymousSubject<MessageEvent> {
    // Convert http(s) to ws(s)
    const wsUrl = this.realtimeUrl.replace(/^http/, 'ws');
    const ws = new WebSocket(`${wsUrl}/ws?connectionId=${connectionId}`);

    const observable = new Rx.Observable(
      (obs: Rx.Observer<MessageEvent>) => {
        ws.onmessage = obs.next.bind(obs);
        ws.onerror = obs.error.bind(obs);
        ws.onclose = obs.complete.bind(obs);
        return ws.close.bind(ws);
      }
    );

    const observer: Rx.Observer<Object> = {
      next: (data: Object) => {
        if (ws.readyState === WebSocket.OPEN) {
          this._logger.debug('ws observer send', data);
          ws.send(JSON.stringify(data));
        }
      },
      error: (e) => {
        this._logger.error('ws observer error', e);
      },
      complete: () => {
        this._logger.info('ws observer complete');
      }
    };

    return new AnonymousSubject<MessageEvent>(observer, observable);
  }
}
