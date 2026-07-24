# AGENTS.md

Development environment guidelines and workflows for this repository. Read this
before acting, then organize the work per the sections below.

**What this repo is:** a .NET desktop framework — native OS windows hosting a
Blazor UI via Photino. Solution `Sala.sln` contains two projects:

| Project | Path | Type | Frameworks | Role |
|---|---|---|---|---|
| `ViewEngine` | `ViewEngine/ViewEngine.csproj` | Library (NuGet `ViewEngine`) | `net8.0;net9.0` | Blazor-on-Photino engine (built on `Photino.NET`) |
| `ViewCore`   | `ViewCore/ViewCore.csproj`     | `WinExe` (Razor SDK)          | `net8.0`        | Sample host app; references `ViewEngine`; Tailwind CSS v4 UI |

> "Package" below means a **.NET project** (`.csproj`), the unit this solution
> is organized around.

## Template docs (`./Docs/`) — reading order

This repo is being shaped into a **"Photino.Blazor Custom Template"**: a hybrid
that scaffolds a **Photino.Blazor desktop UI + an ASP.NET Core Web API backend**.
The spec for that template lives in `./Docs/`, written in **Glyph Markup** (an
instruction-annotation notation for AI, not the IDE). Read them in this order
before generating or incrementing template code:

1. `Docs/GLYPH_LEGEND.md` — decoder for the Glyph notation (read first)
2. `Docs/ARCHITECTURE.md` — layers & conventions (`§UI` + `§API`)
3. `Docs/DATA_MODEL.md` — entities, types, database (API layer)
4. `Docs/BUSINESS_RULES.md` — business rules & calculated fields
5. `Docs/API_ENDPOINTS.md` — API endpoints + how the UI consumes them
6. `Docs/DEPENDENCIES.md` — NuGet packages + UI toolchain (Tailwind)
7. `Docs/TESTS.md` — Service tests (API) + component tests (UI)
8. `Docs/DOCKER.md` — backend dockerization (the desktop UI is **not** containerized)
9. `Docs/DECISIONS.md` — ADRs (do not contradict an accepted ADR without a new one)
10. `Docs/CHANGELOG.md` — history (update after changes)

Rules for working with the template:
- **Do not alter the base** (`ViewEngine`/`ViewCore` source) — increment around it
  with new files/projects (see `Docs/DECISIONS.md` ADR-004).
- `Docs/*` are **Glyph-native**; keep code/tables inside `[OFF]…[ON]` regions when
  editing them, and fill `[PH: …]` placeholders with project-specific data.
- The IDE does not need `./Docs/`; only this AI chain (`CLAUDE.md → AGENTS.md`) does.

## Dev environment tips

- **Locate a project instead of scanning:** `dotnet sln Sala.sln list`.
- **Add a project to the solution:** `dotnet sln Sala.sln add <path>/<Name>.csproj`.
- **Scaffold a new project:** `dotnet new <template> -n <Name>` (e.g. `classlib`,
  `xunit`), then add it with the command above and wire references via
  `dotnet add <consumer>.csproj reference <Name>.csproj`.
- **Confirm the correct package/project name:** check `<PackageId>` /
  `<AssemblyName>` in the `.csproj` (the library's `PackageId` is `ViewEngine`),
  or run `dotnet sln Sala.sln list`. Note: the **root namespace is `ViewEngine`**
  even though the package `Title` and some legacy artifacts still say
  `Photino.Blazor` — trust the source namespace.

## Testing instructions

- **CI plan:** none yet — there is no `.github/workflows` pipeline. `dotnet build`
  is currently the authoritative gate. If you add CI, document its location here.
- **Run every check for a project:** `dotnet build ViewEngine/ViewEngine.csproj`
  (add `dotnet format ViewEngine/ViewEngine.csproj --verify-no-changes` for style).
- **Run tests from the solution root:** `dotnet test Sala.sln`
  (there is **no test project yet**, so this reports zero tests — see the
  requirement below).
- **Focus one test:** `dotnet test --filter "FullyQualifiedName~<TestName>"`
  (applies once a test project exists).
- **MANDATORY:** the solution must **build clean (no build/type errors, no new
  warnings) before merge.** `dotnet build Sala.sln` must succeed.
- **REQUIRED:** add or update tests for changed logic. Since none exist,
  create an xUnit project (`dotnet new xunit -n ViewEngine.Tests`) when you add
  testable behavior, and add it to `Sala.sln`.

## PR instructions

- **Title format:** `[<project>] <Title>` — e.g. `[ViewEngine] Fix dispatcher
  deadlock`, `[ViewCore] Wire Tailwind @source scanning`.
- **MANDATORY before committing:** run `dotnet format Sala.sln` and
  `dotnet build Sala.sln` (plus `dotnet test Sala.sln` once tests exist); all
  must pass. **Base branch for PRs is `master`.**

## Code standards

- **Language / style:** C# on .NET 8/9. `ViewEngine` multi-targets
  `net8.0;net9.0`; `ViewCore` targets `net8.0`. Match surrounding style and
  run `dotnet format`. Use `async`/`await` for asynchronous APIs; no secrets in
  source (use config / environment variables).
- **Non-negotiable:** keep `ViewEngine` multi-targeting **both** `net8.0` and
  `net9.0`; keep `ViewCore` referencing the `ViewEngine` **project** (not a stale
  NuGet copy); solution must build.
- **Do not:** edit generated output — `bin/`, `obj/`, `*/scopedcss/*`, or
  `ViewCore/wwwroot/index.css` (generated by the Tailwind CLI from
  `wwwroot/imports.css`); commit `node_modules/`; or reintroduce `--watch` into
  the `TailwindCSS` MSBuild target (it makes the build hang forever).

## Troubleshooting

- **Build hangs at compile:** the `TailwindCSS` target in `ViewCore.csproj` must
  run the CLI **without** `--watch`. Use `--watch` only in a separate dev
  terminal.
- **Tailwind classes in `.razor` don't apply:** the generated `index.css` was
  built without scanning components. Ensure `wwwroot/imports.css` has
  `@import "tailwindcss";` (v4 syntax, not the v3 `@tailwind` directives) and
  `@source "../**/*.razor";`, then rebuild the CSS.
- **NuGet restore fails for a Photino feed:** the Azure `PhotinoPackages` source
  in `nuget.config` is commented out; restore uses nuget.org. Uncomment it only
  if you need pre-release Photino builds.
- **WARN — naming drift:** source namespace is `ViewEngine`, but stale
  `obj/**/Photino.Blazor.AssemblyInfo.cs` and the package `Title`/repo URL still
  reference `Photino.Blazor`. Don't rely on the old name; a clean rebuild
  regenerates the correct artifacts.

## Notes

- **NT:** this repo is mid-rename from **Photino.Blazor → ViewEngine / `Sala.sln`**.
  Git status shows the old `Photino.Blazor.sln` and `Photino.Blazor/` directory
  being removed — do not resurrect them.
- **NT:** the template is **hybrid** (Photino.Blazor UI + ASP.NET Core API), so
  the API-layer stack (EF Core, controllers, Swagger, Docker) is legitimate for
  the **API** side — see `Docs/` for how the two layers coexist. The legacy
  `.claude/CLAUDE.md` describes only the API half; when it conflicts with the
  actual repo or the `Docs/` set, **trust this file and `./Docs/`.**
- **PRIORITY:** keep the `ViewEngine` library building on both `net8.0` and
  `net9.0`; use `ViewCore` as the sample host to validate UI/runtime changes end
  to end (`dotnet run --project ViewCore`).
