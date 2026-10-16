# Bugbot review rules

Report real bugs, security issues and regressions. Skip style nitpicks
and preference-only suggestions.

## Generated and vendored files

Do not comment on these paths; they are generated, vendored or lockfiles:

- `**/dist/**`
- `**/build/**`
- `**/out/**`
- `**/*.min.js`
- `**/package-lock.json`
- `**/bun.lock`
- `**/bun.lockb`
- `**/yarn.lock`
- `**/Cargo.lock`
- `**/*.snap`

## Path-specific rules

### `**/*.cs`

Follow .NET coding conventions. Flag missing async/await,
ConfigureAwait(false) in library code, IDisposable patterns,
and any nullable reference annotation gaps.

### `**/*.tsx`

Flag missing key props in lists, useEffect dependency issues,
accessibility gaps (alt, aria-*), and any prop drilling that
could be replaced with context.

### `**/*.go`

Flag missing error checks, channel leaks, goroutine leaks,
and any context cancellation issues.

### `**/*.rs`

Flag unwrap()/expect() in non-test code, missing Drop impls,
and any unnecessary allocations.

### `**/*.py`

Flag bare except clauses, mutable default arguments, and
any synchronous I/O in async functions.

### `.github/workflows/**/*.yml`

Flag any user-controlled input interpolated directly in
run: blocks (workflow injection risk). Recommend env: + quoting.
