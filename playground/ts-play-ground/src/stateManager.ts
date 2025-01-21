import { finalize, merge, mergeScan, Observable, of, scan, shareReplay, Subject, switchMap, tap } from "rxjs";
import { v4 as uuidv4 } from 'uuid';

export interface StateSubscription { 
    readonly id: string;
    readonly name: string;
}

export interface State { 
    readonly subscribers: readonly StateSubscription[];
}

export const addSubscriber = (state: State, subscriber: StateSubscription): State => {
    return {
        ...state,
        subscribers: [...state.subscribers, subscriber]
    }
} 

export const removeSubscriber = (state: State, subscriberId: string): State => {
    return {
        ...state,
        subscribers: state.subscribers.filter((s) => s.id !== subscriberId)
    }
} 

type StateAction = (state: State) => Observable<State>;

export class StateManager {
    
    private readonly actions$ = new Subject<StateAction>();

    private readonly state$: Observable<State>;

    constructor() {
        const initialState: State = { subscribers: [] };

        this.state$ = 
            merge(
                this.actions$
            ).pipe(
                mergeScan(
                    // accumulator function
                    (state, action) => action(state),
                    // initial state
                    initialState
                ),
                shareReplay(1)
            )

        this.state$.subscribe({
            next: (x: State) => {
               // console.log('State:', x);
            },
            error: (err) => {
                console.error('Error:', err);
            },
            complete: () => {
                console.log('Completed');
            }
        });
    }

   

    public subscribe(name: string): Observable<State> {
        const subscriber: StateSubscription = { 
            id: uuidv4(), 
            name: name 
        };

        return of(subscriber).pipe(
            tap((s: StateSubscription): void => {
                const action = (state: State) => of(addSubscriber(state, s));
                this.actions$.next(action);
            }),
            switchMap((s: StateSubscription) => this.getSubscription(s.id))
            //switchMap((s: StateSubscription) => this.getSubscriptionObservable(s.id))
        )
    }


    getSubscription(id: string): Observable<State> {
        return this.state$.pipe(
            finalize(() => { 
                console.log('Unsubscribed');
                const action = (state: State) => of(removeSubscriber(state, id));
                this.actions$.next(action);
            })
        )
    }

    getSubscriptionObservable(id: string): Observable<State> {
        return new Observable<State>((subscriber) => {
            const subscription = this.state$.subscribe({
                next: (state: State) => {
                    subscriber.next(state);
                },
                error: (err) => {
                    subscriber.error(err);
                },
                complete: () => {
                    console.log('Subscription completed');
                    subscriber.complete();
                }
            });
    
            return () => {
                subscription.unsubscribe();
                console.log('Unsubscribed');
                const action = (state: State) => of(removeSubscriber(state, id));
                this.actions$.next(action);
            };
        });
    }
}


