# @dd/web

New Vue 3 + TypeScript frontend for the DDTank web migration.

## Migration rules

- The existing `Web/` PHP application remains untouched during the migration.
- Flash assets and the Flash client/protocol are out of scope and must not be modified.
- New features should communicate with `apps/api` instead of accessing SQL directly.
- Database access will eventually be hidden behind a save/persistence boundary.
