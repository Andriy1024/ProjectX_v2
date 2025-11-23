# Backend CI/CD Pipeline Documentation

This document describes the Continuous Integration (CI) pipeline for the ProjectX backend, defined in `.github/workflows/build-backend.yml`.

## Overview

The pipeline is designed to be efficient and scalable by using **selective builds**. It only builds Docker images for microservices that have actually changed, or for all services if shared/common code is modified.

### Triggers

The pipeline runs automatically on:
- **Push** to the `master` branch.
- **Pull Request** targeting the `master` branch.
- **Manual Dispatch** (via GitHub Actions UI).

It is filtered to only trigger when files in the `backend/` directory or the workflow file itself are changed.

---

## Pipeline Stages

The workflow consists of three main jobs:

### 1. Detect Changes & Generate Matrix (`detect-changes`)

This job analyzes the commit to determine which parts of the system need to be rebuilt.

*   **Tools Used**: `dorny/paths-filter` and `jq`.
*   **Logic**:
    1.  It checks for changes in specific paths defined in the `filters` section:
        *   `common`: Changes to `backend/Common/`, `.sln` files, or `Directory.Build.props`.
        *   `dashboard`, `identity`, `filestorage`, etc.: Changes to specific service directories.
    2.  **Matrix Generation**:
        *   If **Common** code changed: The pipeline generates a build matrix containing **ALL** services.
        *   If only **Service** code changed: The pipeline generates a matrix containing **ONLY** the changed services.
        *   If **No relevant code** changed: The matrix is empty, and subsequent jobs are skipped.
*   **Outputs**:
    *   `matrix`: A JSON string defining the services to build (name, dockerfile path, image name).
    *   `has-changes`: Boolean flag (`true`/`false`) indicating if any build is required.

### 2. Build .NET Backend (`build`)

This job compiles and tests the entire .NET solution to ensure code quality before attempting Docker builds.

*   **Prerequisite**: Runs only if `has-changes` is `true`.
*   **Steps**:
    1.  **Setup .NET**: Installs .NET 7.0 SDK.
    2.  **Restore**: Runs `dotnet restore ProjectX.sln`.
    3.  **Build**: Runs `dotnet build` in Release configuration.
    4.  **Test**: Runs `dotnet test` to execute all unit and integration tests.

### 3. Build Docker Images (`docker-build`)

This job builds the Docker images for the services identified in the `detect-changes` job.

*   **Prerequisite**:
    *   The `build` job must pass successfully.
    *   Must be a `push` event to `master` (PRs do not build Docker images by default).
    *   `has-changes` must be `true`.
*   **Strategy**: Uses a **Matrix Strategy** to run builds in parallel for each service defined in the generated matrix.
*   **Steps**:
    1.  **Setup Docker Buildx**: Sets up the advanced Docker builder.
    2.  **Build Image**: Builds the Docker image using the specific `Dockerfile` for the service.
    3.  **Caching**: Uses Docker registry caching (`type=registry`) to speed up future builds.
    4.  **Pushing**:
        *   **Current State**: Pushing to Docker Hub is **DISABLED** (`push: false`).
        *   **To Enable**: Uncomment the "Log in to Docker Hub" step and set `push: true` in the workflow file.

---

## How to Enable Docker Push

To enable pushing images to the Docker Hub registry:

1.  Open `.github/workflows/build-backend.yml`.
2.  Uncomment the login step in the `docker-build` job:
    ```yaml
    # - name: Log in to Docker Hub
    #   uses: docker/login-action@v3
    #   with:
    #     username: andriy1024
    #     password: ${{ secrets.DOCKER_TOKEN }}
    ```
3.  Change the `push` parameter in the "Build Docker image" step to `true`:
    ```yaml
    - name: Build ${{ matrix.name }} Docker image
      uses: docker/build-push-action@v5
      with:
        # ...
        push: true  # <--- Change this to true
    ```
4.  Ensure the `DOCKER_TOKEN` secret is set in the GitHub repository settings.

## Service Configuration

The list of services is defined in the `Generate Matrix` step of the `detect-changes` job. To add a new service:

1.  Add a new filter in the `filters` section of `detect-changes`.
2.  Add the service configuration object to the `SERVICES` JSON array in the `Generate Matrix` script:
    ```json
    {"name": "new-service", "dockerfile": "./path/to/Dockerfile", "image": "repo/image-name"}
    ```
