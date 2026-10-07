// The identity stores use Dapper's runtime mapping on purpose: Dapper.AOT needs an explicit
// compatibility experiment (see AGENTS.md), so it stays off in this host.
[module: Dapper.DapperAot(false)]
