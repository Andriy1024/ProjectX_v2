"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const stateManager_1 = require("./stateManager");
console.log('Hello, TypeScript!');
const stateManager = new stateManager_1.StateManager();
const subscription1 = stateManager
    .subscribe('1')
    .subscribe((state) => {
    console.log('Subscriber 1 Handler:');
    for (const sub of state.subscribers) {
        console.log(sub.id + ' ' + sub.name);
    }
});
const subscription2 = stateManager
    .subscribe('2')
    .subscribe((state) => {
    // here should be state that include subscriber 1 message
    console.log('Subscriber 2 Handler:');
    for (const sub of state.subscribers) {
        console.log(sub.id + ' ' + sub.name);
    }
});
subscription1.unsubscribe();
console.log(`End of script`);
//# sourceMappingURL=index.js.map