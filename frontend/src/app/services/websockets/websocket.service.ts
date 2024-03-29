import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import * as Rx from 'rxjs';
import { map, switchMap, tap } from 'rxjs';
import { AnonymousSubject } from 'rxjs/internal/Subject';
import { REALTIME_API_URL } from 'src/app/app-injection-tokens';
import { IRealtimeMessage } from 'src/app/models/realtime.model';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  constructor(
    private readonly _http: HttpClient,
    @Inject(REALTIME_API_URL)
    private readonly realtimeUrl: string,
  ){}

  private subject: Rx.Subject<MessageEvent> | null = null;

  public subscribe(): Rx.Observable<IRealtimeMessage<any>> {
    if (this.subject && !this.subject.closed) {
      return this.subject.pipe(
        tap(console.log),
        map(r => JSON.parse(r.data) as IRealtimeMessage<any>)
      );
    }

    return this._http
      .post<{connectionId: string}>(`http://localhost:5211/api/realtime/connect`, {})
        .pipe(
          switchMap((response: { connectionId: string }) => {
            this.subject = this.createWS(response.connectionId);
            return this.subject.pipe(
              map(r => JSON.parse(r.data) as IRealtimeMessage<any>)
            );
          })
        );
  }

  private createWS(connectionId: string): AnonymousSubject<MessageEvent> {
    const ws = new WebSocket(`ws://localhost:5211/ws?connectionId=${connectionId}`);

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
          console.log('ws observer send');
          ws.send(JSON.stringify(data));
        }
      },
      error: (e) => {
        console.log('ws observer')
        console.log(e)
      },
      complete: () => {
        console.log('ws observer')
        console.log('complete')
      }
    };

    return new AnonymousSubject<MessageEvent>(observer, observable);
  }
}
