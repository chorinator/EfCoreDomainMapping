# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Code Navigation

Always use codegraph tools for exploration. Never use Bash `find`/`grep`/`cat` — codegraph indexes the workspace and is faster and cheaper.

| Tool | When to use |
|------|-------------|
| `codegraph_explore` | **Primary — use first.** Natural language or symbol/file names → verbatim source grouped by file. "How does X work", architecture, "where is X", surveying an area, or before an edit to see blast radius. Usually the only call needed. |
| `codegraph_node` | **Read a file** (pass `file` only) — replaces the Read tool; returns source + line numbers + dependents. Or **single named symbol** (pass `symbol`) — location, signature, source, caller/callee trail before editing it. |
| `codegraph_search` | **Locate a symbol by name** → file:line only, no code. Use to check existence or find where to look before a `node`/`explore` call. |
| `codegraph_callers` | **Who calls X?** Lists callers of a function/method/class. Use to understand blast radius before a breaking change. |

## Commands

```bash
# Build
dotnet build

# Run console app
dotnet run --project src/EfCoreMapping.Presentation.Console
```

No tests project exists yet (`tests/` directory is empty).

## Architecture

Exploration sandbox for mapping rich domain value objects to a Postgres schema using EF Core. Three layers:

**Domain** (`src/EfCoreMapping.Domain`) — pure, no EF dependency.
- `Transfer` — aggregate root. Has a strongly-typed `TransferId` (wraps `Guid`), a `Money` value object, and a `Timestamp` (enforces UTC).
- `Money` — complex value object wrapping `decimal Amount` + `Currency`. Rounds `Amount` to `Currency.DecimalPlaces` on construction.
- `Currency` — record with `Code` + `DecimalPlaces`. Static instances: `USD`, `EUR`, `GBP`, `JPY`.
- `Timestamp` — enforces UTC; rejects non-UTC `DateTime` at construction.
- All domain types include a private parameterless constructor required by EF Core.

**Infrastructure.EfCore** (`src/Infrastructure/EfCoreMapping.Infrastructure.EfCore`) — provider-agnostic.
- `AppDbContext` — abstract, exposes `DbSet<Transfer>`.
- `Specifications/` — static demo helpers (`InsertTransfersDemo`, `QueryTransfersDemo`) showing how complex-property queries translate to SQL.

**Infrastructure.Postgres** (`src/Infrastructure/EfCoreMapping.Infrastructure.Postgres`) — Postgres-specific.
- `PostgresAppDbContext` — concrete subclass. Defines the full column mapping in `OnModelCreating`:
  - Shadow `int Key` as the physical PK (auto-increment); `TransferId` stored as `uniqueidentifier PublicId` with a unique index.
  - `Money` mapped via `ComplexProperty` (owned type, same table); `Currency` nested as another `ComplexProperty`.
  - `Timestamp` ↔ `datetime2` via `TimestampConverter` (strips/restores `DateTimeKind.Utc`).
  - `TransferId` ↔ `Guid` via `TransferIdConverter`.

**Key design decisions:**
- Domain types have no EF attributes — all mapping is in `PostgresAppDbContext.OnModelCreating`.
- `Money` and `Currency` use EF Core's `ComplexProperty` (not owned entities), so they share the `Transfer` row without a join.
- Physical PK is a shadow `int` key; the domain-visible `TransferId` (GUID) is a unique index column, keeping insert performance on sequential int while exposing a stable public identifier.

**Connection string** — hardcoded in `Program.cs` pointing to `localhost:5432/transfers`. Update before running.
