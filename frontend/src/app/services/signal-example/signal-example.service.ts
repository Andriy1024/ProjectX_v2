import { Injectable, signal, computed } from '@angular/core';

export interface CounterState {
  value: number;
  lastUpdated: Date;
}

/**
 * Example service using modern Angular Signals
 * This demonstrates state management with signals instead of BehaviorSubject/Observable
 */
@Injectable({
  providedIn: 'root'
})
export class SignalExampleService {
  // Writable signal - like BehaviorSubject but simpler
  private readonly _counter = signal<CounterState>({ 
    value: 0, 
    lastUpdated: new Date() 
  });

  // Readonly signal for consumers
  public readonly counter = this._counter.asReadonly();

  // Computed signal - automatically recalculates when dependencies change
  public readonly isEven = computed(() => this._counter().value % 2 === 0);
  public readonly isPositive = computed(() => this._counter().value > 0);
  public readonly doubledValue = computed(() => this._counter().value * 2);

  // Methods to update state
  public increment(): void {
    this._counter.update(state => ({
      value: state.value + 1,
      lastUpdated: new Date()
    }));
  }

  public decrement(): void {
    this._counter.update(state => ({
      value: state.value - 1,
      lastUpdated: new Date()
    }));
  }

  public reset(): void {
    this._counter.set({
      value: 0,
      lastUpdated: new Date()
    });
  }

  /**
   * Benefits of Signals over Observables:
   * 1. No need for async pipe in templates
   * 2. Automatic subscription management (no unsubscribe needed)
   * 3. Computed values are cached and only recalculate when needed
   * 4. Better performance with fine-grained reactivity
   * 5. Simpler API - no operators, pipes, or subscription management
   */
}
