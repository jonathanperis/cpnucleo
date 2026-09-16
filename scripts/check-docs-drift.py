#!/usr/bin/env python3
"""Check current public documentation contracts without freezing test-case totals."""
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8")


def require(condition, message):
    if not condition:
        raise SystemExit(f"docs drift: {message}")


def main():
    readme = read("README.md")
    major = json.loads(read("global.json"))["sdk"]["version"].split(".")[0]
    require(f".NET {major}" in readme, "README runtime differs from global.json")
    counts = (
        len(list((ROOT / "src/WebApi/Endpoints").glob("**/Endpoint.cs"))),
        len(list((ROOT / "src/GrpcServer/Handlers").glob("**/*Handler.cs"))),
        len(list((ROOT / "src/GrpcServer.Contracts/Commands").glob("**/*Command.cs"))),
    )
    require(counts[0] > 0 and len(set(counts)) == 1, f"transport resource counts differ: {counts}")
    require(f"Both transports expose {counts[0]} CRUD operations" in readme, "README endpoint count is stale")
    require("Five test projects" in readme, "README should identify the test-suite map")
    require(len(list((ROOT / "tests").glob("*/*.csproj"))) == 5, "update the documented test-project map")

    package = json.loads(read("src/WebClient/package.json"))
    dependencies = {**package.get("dependencies", {}), **package.get("devDependencies", {})}
    require("astro" in dependencies, "the documented Astro frontend is missing")
    require(not any("qwik" in name for name in dependencies), "the native Astro baseline must not reintroduce its removed rendering runtime")

    sidebar = read("docs/src/lib/sidebar.config.ts")
    page = read("docs/src/pages/docs/[...slug].astro")
    labels = page.split("const SLUG_LABEL", 1)[1].split("const DOC_SUMMARIES", 1)[0]
    summaries = page.split("const DOC_SUMMARIES", 1)[1].split("const globResult", 1)[0]
    slugs = {path.stem for path in (ROOT / "docs/wiki").glob("*.md")}
    for path in (ROOT / "docs/wiki").glob("*.md"):
        slug = path.stem
        require(f'"{slug}"' in sidebar, f"{slug} is missing from navigation")
        require(re.search(rf"(?:^|\n)\s*'?{re.escape(slug)}'?:", labels), f"{slug} needs a page label")
        require(re.search(rf"(?:^|\n)\s*'?{re.escape(slug)}'?:", summaries), f"{slug} needs a page summary")
        # Individual pages are served at /docs/<slug>/, unlike the home index.
        if slug != "home":
            for target in re.findall(r"\]\(([a-z0-9-]+)\)", path.read_text(encoding="utf-8")):
                require(target not in slugs, f"{slug}: link to {target} must use ../{target}/")

    obsolete = [
        "docker compose -f compose.yaml -f compose.prod.yaml",
        "27 architecture tests",
        "JWT authentication is configured but currently commented out",
        "known compile drift",
    ]
    for path in [ROOT / "README.md", ROOT / "AGENTS.md", *(ROOT / "docs/wiki").glob("*.md")]:
        content = path.read_text(encoding="utf-8")
        for text in obsolete:
            require(text not in content, f"{path.relative_to(ROOT)} contains obsolete guidance: {text}")

    release = read(".github/workflows/main-release.yml")
    for text in ["sha-${{ github.sha }}-amd64", "sha-${{ github.sha }}-arm64", "Deploy to Hostinger Docker Manager", "scripts/smoke-production.sh"]:
        require(text in release, f"release contract missing: {text}")
    require("--migrate-database" in read("compose.prod.yaml"), "production must run additive migrations")
    require("--run-fake-data-csv-import" not in read("compose.prod.yaml"), "production must not automatically reseed")
    require("/readyz" in read("scripts/smoke-production.sh"), "deployment must verify database readiness")
    print(f"Documentation contracts passed: {counts[0]} operations per transport; test totals come from runners.")


if __name__ == "__main__":
    main()
