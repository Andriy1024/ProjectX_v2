import { State, StateManager } from "./stateManager";
import { IUser } from "./user";

console.log('Hello, TypeScript!');


const stateManager = new StateManager();

const subscription1 = stateManager
    .subscribe('1')
    .subscribe((state: State) => {
        console.log('Subscriber 1 Handler:');

        for (const sub of state.subscribers) {
            console.log(sub.id + ' ' + sub.name);
        }
    });

const subscription2 = stateManager
    .subscribe('2')
    .subscribe((state: State) => {
        // here should be state that include subscriber 1 message
        console.log('Subscriber 2 Handler:');

        for (const sub of state.subscribers) {
            console.log(sub.id + ' ' + sub.name);
        }
    });


subscription1.unsubscribe();

console.log(`End of script`);