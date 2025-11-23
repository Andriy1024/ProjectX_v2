# Modern Angular Features: Signals vs Observables

This project demonstrates both **modern Angular Signals** and **traditional RxJS Observables** to show the evolution of state management in Angular.

## Signals Implementation (Modern Approach)

### Components Using Signals

#### 1. **TodosComponent** - Full Signals Implementation
```typescript
// Using inject() instead of constructor injection
private readonly _todoService = inject(TodoService);

// Convert Observable to Signal using toSignal()
public readonly todos = toSignal(this._todoService.getTodos(), { initialValue: [] });

// Computed signals - auto-update when dependencies change
public readonly completedCount = computed(() => 
  this.todos().filter(todo => todo.completed).length
);
```

**Template Usage:**
```html
<!-- No async pipe needed! -->
@for (todo of todos(); track todo.id) {
  <div>{{ todo.name }}</div>
}

<!-- Computed signals work the same way -->
<div>{{ completedCount() }} completed</div>
```

#### 2. **BookmarksComponent** - Signals with Computed Values
```typescript
public readonly bookmarks = toSignal(this._bookmarkService.getBookmarks(), { initialValue: [] });
public readonly bookmarkCount = computed(() => this.bookmarks().length);
```

#### 3. **AppComponent** - Authentication State with Signals
```typescript
// No need for ngOnInit, ngOnDestroy, or manual subscriptions!
public readonly isAuthenticated = toSignal(
  this._authService.isAuthenticated(), 
  { initialValue: false }
);
```

### Benefits of Signals

✅ **No Async Pipe** - Direct value access in templates
✅ **Automatic Subscription Management** - No unsubscribe needed
✅ **Computed Values** - Cached and only recalculate when dependencies change
✅ **Better Performance** - Fine-grained reactivity, only updates what changed
✅ **Simpler API** - No RxJS operators, pipes, or subscription management
✅ **Type Safety** - Better TypeScript inference
✅ **No Memory Leaks** - Framework handles cleanup automatically

### Signal APIs Used

1. **`toSignal(observable)`** - Converts Observable to Signal
2. **`signal(value)`** - Creates a writable signal
3. **`computed(() => ...)`** - Creates derived/computed values
4. **`inject(Service)`** - Modern dependency injection
5. **`.asReadonly()`** - Exposes signal as readonly

## Observable Implementation (Traditional Approach)

### Components Using Observables

#### 1. **NotesComponent** - Classic Observable Pattern
```typescript
public notes$: Observable<Note[]> = from([]);

constructor(
  private readonly _noteService: NoteService,
  private readonly _router: Router
) { }

public ngOnInit(): void {
  this.notes$ = this._noteService.getNotes();
}
```

**Template Usage:**
```html
<!-- Requires async pipe -->
@for (note of notes$ | async; track note) {
  <div>{{ note.title }}</div>
}
```

#### 2. **BookmarksManageComponent** - Observable with SwitchMap
```typescript
this.bookmarks$ = this.route.paramMap.pipe(
  switchMap((paramMap) => {
    return this.bookmarkService.getBookmarks();
  })
);
```

### When to Use Observables

- Complex async operations (HTTP requests, WebSockets)
- Stream transformations (map, filter, debounce, etc.)
- Interop with existing RxJS-based code
- Backend integration (Angular HttpClient returns Observables)

## Comparison Table

| Feature | Signals | Observables |
|---------|---------|-------------|
| **Subscription** | Automatic | Manual (async pipe or subscribe) |
| **Cleanup** | Automatic | Manual (unsubscribe in ngOnDestroy) |
| **Template Syntax** | `value()` | `value$ \| async` |
| **Computed Values** | `computed()` | `combineLatest` + map |
| **Performance** | Fine-grained updates | Zone-based change detection |
| **Learning Curve** | Easier | Steeper (RxJS operators) |
| **Use Case** | State management, sync values | Async streams, HTTP, events |

## Example Service: Signals vs Observables

### Signal-Based Service
```typescript
@Injectable({ providedIn: 'root' })
export class SignalExampleService {
  private readonly _counter = signal(0);
  public readonly counter = this._counter.asReadonly();
  public readonly doubled = computed(() => this._counter() * 2);

  increment() {
    this._counter.update(v => v + 1);
  }
}
```

### Observable-Based Service
```typescript
@Injectable({ providedIn: 'root' })
export class ObservableExampleService {
  private readonly _counter$ = new BehaviorSubject(0);
  public readonly counter$ = this._counter$.asObservable();
  public readonly doubled$ = this.counter$.pipe(map(v => v * 2));

  increment() {
    this._counter$.next(this._counter$.value + 1);
  }
}
```

## Migration Strategy

### Converting Observable to Signal

**Before (Observable):**
```typescript
export class MyComponent implements OnInit, OnDestroy {
  data$: Observable<Data[]>;
  subscription: Subscription;

  constructor(private service: DataService) {}

  ngOnInit() {
    this.data$ = this.service.getData();
  }

  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }
}
```

**After (Signal):**
```typescript
export class MyComponent {
  private service = inject(DataService);
  data = toSignal(this.service.getData(), { initialValue: [] });
}
```

## Best Practices

### Use Signals For:
- Component state (loading, error states)
- UI state (selected items, filters)
- Derived/computed values
- Simple synchronous state

### Use Observables For:
- HTTP requests
- WebSocket streams
- Complex async operations
- Event streams
- Integration with existing RxJS code

### Hybrid Approach (Recommended):
- Use Observables for data fetching (services)
- Convert to Signals in components with `toSignal()`
- Get best of both worlds: powerful streams + simple state management

## File Structure

```
Components with Signals:
├── app.component.ts - Authentication state
├── components/todos/todos.component.ts - Full signals implementation
└── bookmarks/bookmarks.component.ts - Signals with computed values

Components with Observables (for comparison):
├── components/notes/notes.component.ts - Classic Observable pattern
└── bookmarks-manage/bookmarks-manage.component.ts - Observable with operators

Services:
├── services/signal-example/signal-example.service.ts - Signal-based state
└── services/notification/notification.service.ts - Observable-based events
```

## Key Takeaways

1. **Signals are the future** - Angular is moving towards signal-based reactivity
2. **Observables aren't going away** - Still essential for async operations
3. **Use both together** - `toSignal()` bridges the gap
4. **Incremental migration** - Convert components gradually
5. **Better performance** - Signals enable fine-grained change detection
6. **Simpler code** - Less boilerplate, no subscription management

## Angular Version
This implementation uses Angular 21 features including:
- Signals API
- `toSignal()` from `@angular/core/rxjs-interop`
- `computed()` for derived state
- `inject()` function for dependency injection
- Standalone components
