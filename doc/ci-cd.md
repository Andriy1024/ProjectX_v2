# Backend CI/CD Pipeline Documentation

This document provides a detailed explanation of the Continuous Integration (CI) pipeline for the ProjectX backend, defined in `.github/workflows/build-backend.yml`.

## 1. Workflow Triggers

The pipeline is configured to run automatically on specific events.

```yaml
on:
  push:
    branches: [ master ]
    paths:
      - 'backend/**'
      - '.github/workflows/build-backend.yml'
  pull_request:
    branches: [ master ]
    paths:
      - 'backend/**'
      - '.github/workflows/build-backend.yml'
  workflow_dispatch:
```

**Explanation:**
*   **`push`**: Triggers when code is pushed to the `master` branch, but ONLY if files in `backend/` or the workflow file itself have changed.
*   **`pull_request`**: Triggers when a PR is opened against `master`, with the same path filters.
*   **`workflow_dispatch`**: Allows you to manually trigger the pipeline from the GitHub Actions "Run workflow" button.

---

## 2. Job: Detect Changes (`detect-changes`)

This is the "brain" of the pipeline. It determines which microservices need to be rebuilt based on the files changed in the commit.

### Step: Define Filters

We use `dorny/paths-filter` to map file paths to logical names (filters).

```yaml
- name: Check for changes
  uses: dorny/paths-filter@v3
  id: filter
  with:
    filters: |
      common:
        - 'backend/Common/**'
        - 'backend/Directory.*.props'
        - 'backend/*.sln'
      dashboard:
        - 'backend/Services/ProjectX.Dashboard/**'
      identity:
        - 'backend/Services/ProjectX.Identity/**'
      # ... (other services)
```

**Explanation:**
*   **`common`**: Represents shared code. If anything here changes, we assume *everything* might be affected.
*   **`dashboard`, `identity`, etc.**: Specific folders for each microservice.

### Step: Generate Dynamic Matrix

This script generates a JSON matrix that tells the next jobs what to build.

```yaml
- name: Generate Matrix
  id: set-matrix
  run: |
    # 1. Define all services configuration
    SERVICES='[
      {"name": "dashboard", "dockerfile": "./backend/Services/ProjectX.Dashboard/ProjectX.Dashboard.API/Dockerfile", "image": "andriy1024/projectx-dashboard"},
      {"name": "identity", "dockerfile": "./backend/Services/ProjectX.Identity/ProjectX.Identity.API/Dockerfile", "image": "andriy1024/projectx-identity"},
      ...
    ]'
    
    # 2. Get list of changed filters from previous step
    CHANGED_FILTERS='${{ steps.filter.outputs.changes }}'
    
    # 3. Logic: Common vs Specific
    if echo "$CHANGED_FILTERS" | grep -q "common"; then
      # If common code changed, build EVERYTHING
      FINAL_MATRIX="$SERVICES"
    else
      # Otherwise, filter the SERVICES JSON to keep only changed ones
      FINAL_MATRIX=$(echo "$SERVICES" | jq --argjson changes "$CHANGED_FILTERS" 'map(select([.name] | inside($changes)))')
    fi
    
    # 4. Output results
    echo "matrix={\"include\":$FINAL_MATRIX}" >> $GITHUB_OUTPUT
```

**Explanation:**

This step uses a **Bash script** (indicated by `run: |`) to programmatically decide what needs to be built. Here is a breakdown for beginners:

1.  **Defining the "Database" (`SERVICES`)**:
    *   We create a JSON array variable named `SERVICES`.
    *   This acts as a lookup table containing the configuration for every microservice (its name, where its Dockerfile is, and what the image should be called).

2.  **Reading Inputs (`CHANGED_FILTERS`)**:
    *   `steps.filter.outputs.changes` comes from the previous "Check for changes" step.
    *   It returns a list of filters that matched, e.g., `['dashboard', 'identity']`.

3.  **The Logic (If/Else)**:
    *   **Scenario A (Common Code)**: The script checks if "common" is in the list of changes. If you changed shared code, we must rebuild *everything* to ensure the changes didn't break anything.
    *   **Scenario B (Specific Service)**: If only specific services changed, we use a tool called `jq` (a command-line JSON processor). It filters the `SERVICES` list to keep *only* the items where the `name` matches our changed filters.

4.  **Outputting Results (`$GITHUB_OUTPUT`)**:
    *   In GitHub Actions, you can't just set a variable to pass it to another job. You must write it to a special file path stored in `$GITHUB_OUTPUT`.
    *   `echo "matrix=..." >> $GITHUB_OUTPUT` saves our calculated JSON so the `docker-build` job can read it later.

---

## 3. Job: Build .NET Backend (`build`)

This job compiles the code and runs tests. It runs *before* Docker builds to ensure code quality.

```yaml
build:
  needs: detect-changes
  if: needs.detect-changes.outputs.has-changes == 'true'
  steps:
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '7.0.x'
    
    - name: Restore dependencies
      working-directory: ./backend
      run: dotnet restore ProjectX.sln
    
    - name: Build solution
      working-directory: ./backend
      run: dotnet build ProjectX.sln --configuration Release --no-restore
    
    - name: Run tests
      working-directory: ./backend
      run: dotnet test ProjectX.sln --configuration Release --no-build --verbosity normal
```

**Explanation:**
*   **`needs: detect-changes`**: Waits for the detection job to finish.
*   **`if: ...has-changes == 'true'`**: Skips this entire job if no relevant files were changed.
*   It performs standard .NET operations: `restore` -> `build` -> `test`.

---

## 4. Job: Build Docker Images (`docker-build`)

This job builds the actual Docker images. It uses a **Matrix Strategy** to run in parallel.

### Strategy Configuration

```yaml
docker-build:
  needs: [detect-changes, build]
  strategy:
    matrix: ${{ fromJson(needs.detect-changes.outputs.matrix) }}
    fail-fast: false
```

**Explanation:**
*   **`matrix: ${{ fromJson(...) }}`**: This is where the magic happens. GitHub Actions parses the JSON output from Job 1 and creates a separate "sub-job" for each item in the `include` list.
*   For example, if `dashboard` and `identity` changed, this job will run twice in parallel: once with `matrix.name = dashboard` and once with `matrix.name = identity`.

### Build Step

```yaml
- name: Build ${{ matrix.name }} Docker image
  uses: docker/build-push-action@v5
  with:
    context: ./backend
    file: ${{ matrix.dockerfile }}
    push: false
    tags: ${{ matrix.image }}:latest,${{ matrix.image }}:${{ github.sha }}
    cache-from: type=registry,ref=${{ matrix.image }}:buildcache
    cache-to: type=registry,ref=${{ matrix.image }}:buildcache,mode=max
```

**Explanation:**
*   **`file: ${{ matrix.dockerfile }}`**: Uses the specific Dockerfile path for the current service in the matrix.
*   **`tags`**: Tags the image with `latest` and the git commit SHA.
*   **`cache-from/to`**: Uses the Docker registry to cache build layers, significantly speeding up builds.
*   **`push: false`**: Currently configured to build but *not* push to Docker Hub.

---

## How to Add a New Service

1.  **Update Filters**: Add a new entry in the `detect-changes` job under `filters`.
    ```yaml
    newservice:
      - 'backend/Services/ProjectX.NewService/**'
    ```
2.  **Update Matrix Script**: Add the service definition to the `SERVICES` JSON array in the `Generate Matrix` step.
    ```json
    {"name": "newservice", "dockerfile": "./path/to/Dockerfile", "image": "user/image"}
    ```
