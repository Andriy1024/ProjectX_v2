"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.StateManager = exports.removeSubscriber = exports.addSubscriber = void 0;
const rxjs_1 = require("rxjs");
const uuid_1 = require("uuid");
const addSubscriber = (state, subscriber) => {
    return Object.assign(Object.assign({}, state), { subscribers: [...state.subscribers, subscriber] });
};
exports.addSubscriber = addSubscriber;
const removeSubscriber = (state, subscriberId) => {
    return Object.assign(Object.assign({}, state), { subscribers: state.subscribers.filter((s) => s.id !== subscriberId) });
};
exports.removeSubscriber = removeSubscriber;
class StateManager {
    constructor() {
        this.actions$ = new rxjs_1.Subject();
        const initialState = { subscribers: [] };
        this.state$ =
            (0, rxjs_1.merge)(this.actions$).pipe((0, rxjs_1.mergeScan)(
            // accumulator function
            (state, action) => action(state), 
            // initial state
            initialState), (0, rxjs_1.shareReplay)(1));
        this.state$.subscribe({
            next: (x) => {
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
    subscribe(name) {
        const subscriber = {
            id: (0, uuid_1.v4)(),
            name: name
        };
        return (0, rxjs_1.of)(subscriber).pipe((0, rxjs_1.tap)((s) => {
            const action = (state) => (0, rxjs_1.of)((0, exports.addSubscriber)(state, s));
            this.actions$.next(action);
        }), (0, rxjs_1.switchMap)((s) => this.getSubscription(s.id))
        //switchMap((s: StateSubscription) => this.getSubscriptionObservable(s.id))
        );
    }
    getSubscription(id) {
        return this.state$.pipe((0, rxjs_1.finalize)(() => {
            console.log('Unsubscribed');
            const action = (state) => (0, rxjs_1.of)((0, exports.removeSubscriber)(state, id));
            this.actions$.next(action);
        }));
    }
    getSubscriptionObservable(id) {
        return new rxjs_1.Observable((subscriber) => {
            const subscription = this.state$.subscribe({
                next: (state) => {
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
                const action = (state) => (0, rxjs_1.of)((0, exports.removeSubscriber)(state, id));
                this.actions$.next(action);
            };
        });
    }
}
exports.StateManager = StateManager;
//# sourceMappingURL=stateManager.js.map