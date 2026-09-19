# Blog.API

A Clean Architecture / CQRS blog API — ASP.NET Core 8, EF Core, MediatR,
SignalR, Serilog. Layout: `Blog.Domain` → `Blog.DAL` → `Blog.Application`
→ `Blog.API` (dependencies point inward only).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) + Docker Compose (recommended — gives
  you a real SQL Server, Redis, and MongoDB locally with one command)

## Getting started (recommended: Docker Compose)

```bash
docker-compose up
```

This starts seven containers: the API itself (`http://localhost:8080`),
SQL Server, Redis, MongoDB, RabbitMQ (management UI at
`http://localhost:15672`, guest/guest), `blog-notifications` — a
separate, independently-deployed service (its own sibling repo, built
from `../blog-notifications`) that consumes new-comment events over
RabbitMQ — and `blog-gateway` (`http://localhost:8082`), a reverse proxy
in front of both services so `blog-blazor` only needs one address. See
`../blog-notifications/README.md`, `../blog-gateway/README.md`, and
`learning-notes/notes/40-monolith-vs-microservices.md` onward for why.
**First run only**, apply the database schema (the containers start with
an empty database — nothing runs migrations automatically):

```bash
dotnet tool install --global dotnet-ef   # if you don't already have it
dotnet ef database update \
  --project Blog.DAL --startup-project Blog.API \
  --connection "Server=localhost,1433;Database=BlogDb;User Id=sa;Password=LocalDev_Only_P@ss1;TrustServerCertificate=True;"
```

Then check `http://localhost:8080/health` — should report healthy once
the schema is applied.

## Getting started (without Docker)

You'll need your own reachable SQL Server instance. Set
`ConnectionStrings:AzureSql` to point at it (via `dotnet user-secrets`,
an environment variable, or directly in `appsettings.json` — see
`learning-notes/notes/36-secrets-management.md` for the reasoning behind
what is and isn't checked in). Redis and MongoDB are optional: without a
`Redis:ConnectionString` configured the app falls back to in-process
caching automatically, and without `Logs:DatabaseType: MongoDB` it just
logs to the console — nothing breaks if you skip them.

```bash
cd Blog.API
dotnet run                         # Development, https profile
dotnet run --launch-profile Staging
```

- Swagger UI: `https://localhost:7163/swagger` (visible in Development
  and Staging, hidden in Production)
- Health check: `/health`

## Running tests

```bash
dotnet test Blog.Application.Test
```

## Configuration & environments

`appsettings.json` (shared) layered with
`appsettings.{Development,Staging,Production}.json`. Full reasoning for
what lives where — and why the local JWT signing key is deliberately
checked into source control — is written up in
`learning-notes/notes/35-environments-config.md` and
`learning-notes/notes/36-secrets-management.md`.

## Learn more

This project doubles as a running C#/.NET learning series — every
non-trivial design decision, real bug found, and feature added is written
up in `../learning-notes/` (`./build.sh` there generates a PDF of the
whole thing).
