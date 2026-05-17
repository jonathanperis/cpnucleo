#!/usr/bin/env python3
"""Source-backed documentation drift checks for README and wiki facts."""
from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8")


def fail(message: str) -> None:
    print(f"docs drift: {message}", file=sys.stderr)
    sys.exit(1)


def assert_contains(path: str, needle: str) -> None:
    if needle not in read(path):
        fail(f"{path} is missing expected text: {needle!r}")


def assert_absent(path: str, needle: str) -> None:
    if needle in read(path):
        fail(f"{path} still contains stale text: {needle!r}")


def count_attrs(path: str, pattern: str) -> int:
    total = 0
    for file in (ROOT / path).rglob("*.cs"):
        total += len(re.findall(pattern, file.read_text(encoding="utf-8")))
    return total


def main() -> None:
    global_json = read("global.json")
    assert '"version": "10.0.102"' in global_json

    compose = read("compose.yaml")
    assert 'image: postgres:16.7' in compose
    assert '"5300:5020"' in compose
    assert '"5301:5021"' in compose
    assert 'image: ghcr.io/jonathanperis/cpnucleo-web-api:latest' in compose

    grpc_program = read("src/GrpcServer/Program.cs")
    assert "ListenAnyIP(5020" in grpc_program and "HttpProtocols.Http2" in grpc_program
    assert "ListenAnyIP(5021" in grpc_program and "HttpProtocols.Http1" in grpc_program

    main_release = read(".github/workflows/main-release.yml")
    assert "${{ matrix.image }}:sha-${{ github.sha }}-amd64" in main_release
    assert "${{ matrix.image }}:sha-${{ github.sha }}-arm64" in main_release
    assert "--tag ${{ matrix.image }}:sha-${{ github.sha }}" in main_release
    assert "images: ${{ matrix.image }}" in main_release
    assert "AZURE_CLIENT_ID" in main_release
    assert "AZURE_WEBAPP_PUBLISH_PROFILE" not in main_release

    deploy = read(".github/workflows/deploy.yml")
    assert "pages-docs-deploy.yml@main" in deploy

    wiki_files = sorted((ROOT / "docs/wiki").glob("*.md"))
    slugs = {p.stem for p in wiki_files}
    sidebar = read("docs/src/lib/sidebar.config.ts")
    for slug in slugs:
        if slug not in sidebar:
            fail(f"docs/wiki/{slug}.md is not represented in sidebar config")

    arch_tests = count_attrs("test/Architecture.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    unit_tests = count_attrs("test/WebApi.Unit.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    integration_tests = count_attrs("test/WebApi.Integration.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    if (arch_tests, unit_tests, integration_tests) != (25, 49, 55):
        fail(f"unexpected test counts: architecture={arch_tests}, unit={unit_tests}, integration={integration_tests}")

    endpoint_count = len(list((ROOT / "src/WebApi/Endpoints").glob("**/Endpoint.cs")))
    handler_count = len(list((ROOT / "src/GrpcServer/Handlers").glob("**/*Handler.cs")))
    command_count = len(list((ROOT / "src/GrpcServer.Contracts/Commands").glob("**/*Command.cs")))
    if (endpoint_count, handler_count, command_count) != (55, 55, 55):
        fail(f"unexpected endpoint/handler/command counts: {endpoint_count}/{handler_count}/{command_count}")

    assert_contains("README.md", "25 architecture tests")
    assert_contains("README.md", "Pages documentation")
    assert_absent("README.md", "github.com/jonathanperis/cpnucleo/wiki")
    assert_absent("docs/wiki/getting-started.md", "http://localhost:5300/healthz")
    assert_contains("docs/wiki/getting-started.md", "http://localhost:5301/healthz")
    assert_contains("docs/wiki/api-reference.md", "gRPC transport: `http://localhost:5300` (HTTP/2)")
    assert_contains("docs/wiki/api-reference.md", "Health check: `http://localhost:5301/healthz` (HTTP/1.1)")
    assert_contains("docs/wiki/testing.md", "55 integration tests")
    assert_contains("docs/wiki/deployment.md", "OIDC")

    print("README/wiki drift checks passed")


if __name__ == "__main__":
    main()
