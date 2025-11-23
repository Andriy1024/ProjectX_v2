# Standalone Components Architecture

This project demonstrates both **standalone components** (modern Angular approach) and **module-based components** (traditional approach).

## Standalone Components (8 components)

These components are self-contained and don't require NgModule declarations:

### Feature Components
- **NotesComponent** - Notes management feature
- **TodosComponent** - Todo list feature  
- **BookmarksComponent** - Bookmarks display feature
- **BookmarksManageComponent** - Bookmarks management feature
- **BookmarkEditComponent** - Individual bookmark editor
- **SignInComponent** - Authentication feature

### Presentational/Utility Components
- **BookmarkTileComponent** - Reusable bookmark tile display
- **NotificationComponent** - Global notification display

**Benefits:**
- ✅ Self-contained with explicit imports
- ✅ Easier to lazy load
- ✅ Simpler testing (no module setup needed)
- ✅ Better tree-shaking
- ✅ More modular and reusable

## Module-Based Components (3 components + 1 module)

These follow the traditional Angular NgModule pattern:

### Root/Shell Components
- **AppComponent** - Application root (kept in module as bootstrap component)
- **TabsComponent** - Navigation tabs (simple shared component)

### Shared Module
- **DynamicFormModule** - Contains `DynamicFormComponent`
  - Complex shared component used across features
  - Good example of when to use a module for shared functionality
  - Exports the component for use in standalone components

**Why keep these as module-based:**
- AppComponent: Root component that bootstraps the app
- TabsComponent: Simple example of module-based approach for comparison
- DynamicFormModule: Demonstrates how modules can export components for use in standalone components

## Architecture Pattern

```
AppModule (root)
├── AppComponent (module-based, bootstrap)
├── TabsComponent (module-based)
├── NotificationComponent (standalone, imported in module)
└── DynamicFormModule (feature module)
    └── DynamicFormComponent (module-based, exported)

Standalone Components (route-level)
├── BookmarksComponent → uses BookmarkTileComponent
├── BookmarksManageComponent
├── BookmarkEditComponent → uses DynamicFormModule
├── NotesComponent
├── TodosComponent
└── SignInComponent → uses DynamicFormModule
```

## Key Learnings

1. **Standalone components can import modules** - DynamicFormModule is imported by standalone components
2. **Modules can import standalone components** - NotificationComponent is imported in AppModule
3. **Mix and match** - You can gradually migrate to standalone while keeping some module-based components
4. **Route-level components** are excellent candidates for standalone
5. **Shared complex components** may benefit from staying in a module (like DynamicFormComponent)

## Migration Strategy

To convert a module-based component to standalone:
1. Add `standalone: true` to @Component decorator
2. Add `imports: []` array with required dependencies (CommonModule, RouterModule, etc.)
3. Remove from NgModule declarations
4. Import directly where needed (routes or other components)

## Future Migration

Consider converting:
- `DynamicFormComponent` → standalone (would need to import ReactiveFormsModule, CommonModule, RouterModule directly)
- `TabsComponent` → standalone (simple component, good candidate)
