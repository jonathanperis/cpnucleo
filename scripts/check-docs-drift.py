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


def require_text(source_name: str, text: str, needle: str, *, present: bool = True) -> None:
    found = needle in text
    if present and not found:
        fail(f"{source_name} is missing expected text: {needle!r}")
    if not present and found:
        fail(f"{source_name} still contains stale text: {needle!r}")


def count_attrs(path: str, pattern: str) -> int:
    total = 0
    for file in (ROOT / path).rglob("*.cs"):
        total += len(re.findall(pattern, file.read_text(encoding="utf-8")))
    return total


def main() -> None:
    global_json = read("global.json")
    require_text("global_json", global_json, '"version": "10.0.102"')

    compose = read("compose.yaml")
    require_text("compose", compose, "image: postgres:16.7")
    require_text("compose", compose, '"5300:5020"')
    require_text("compose", compose, '"5301:5021"')
    require_text("compose", compose, "image: ghcr.io/jonathanperis/cpnucleo-web-api:latest")

    grpc_program = read("src/GrpcServer/Program.cs")
    require_text("grpc_program", grpc_program, "ListenAnyIP(5020")
    require_text("grpc_program", grpc_program, "HttpProtocols.Http2")
    require_text("grpc_program", grpc_program, "ListenAnyIP(5021")
    require_text("grpc_program", grpc_program, "HttpProtocols.Http1")

    main_release = read(".github/workflows/main-release.yml")
    require_text("main_release", main_release, "${{ matrix.image }}:sha-${{ github.sha }}-amd64")
    require_text("main_release", main_release, "${{ matrix.image }}:sha-${{ github.sha }}-arm64")
    require_text("main_release", main_release, "--tag ${{ matrix.image }}:sha-${{ github.sha }}")
    require_text("main_release", main_release, "Deploy to Hostinger Docker Manager")
    require_text("main_release", main_release, "HOSTINGER_API_TOKEN")

    legacy_cloud_terms = [
        "A" + "zure",
        "OI" + "DC",
        "az" + "ure" + "-credential",
        "AZ" + "URE_",
        "az" + "ure" + "/login",
        "az" + "ure" + "/arm-deploy",
        "az" + "ure" + "/webapps-deploy",
        "deploy_" + "az" + "ure",
    ]
    for source_name, text in (
        ("main_release", main_release),
        ("deployment_docs", read("docs/wiki/deployment.md")),
    ):
        for needle in legacy_cloud_terms:
            require_text(source_name, text, needle, present=False)

    deploy = read(".github/workflows/deploy.yml")
    require_text("deploy", deploy, "pages-docs-deploy.yml@main")

    wiki_files = sorted((ROOT / "docs/wiki").glob("*.md"))
    slugs = {p.stem for p in wiki_files}
    sidebar = read("docs/src/lib/sidebar.config.ts")
    sidebar_slugs = set(re.findall(r'"([a-z0-9-]+)"', sidebar))
    for slug in slugs:
        if slug not in sidebar_slugs:
            fail(f"docs/wiki/{slug}.md is not represented in sidebar config")

    arch_tests = count_attrs("tests/Architecture.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    unit_tests = count_attrs("tests/WebApi.Unit.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    integration_tests = count_attrs("tests/WebApi.Integration.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    app_tests = count_attrs("tests/Application.Unit.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    security_tests = count_attrs("tests/Security.Unit.Tests", r"\[(?:[^\]]*,\s*)?(Fact|Theory|Test)(?:\s*[,\(\]])")
    if (arch_tests, app_tests, security_tests, unit_tests, integration_tests) != (55, 4, 11, 53, 55):
        fail(f"unexpected test counts: architecture={arch_tests}, application={app_tests}, security={security_tests}, webapi_unit={unit_tests}, integration={integration_tests}")

    endpoint_count = len(list((ROOT / "src/WebApi/Endpoints").glob("**/Endpoint.cs")))
    handler_count = len(list((ROOT / "src/GrpcServer/Handlers").glob("**/*Handler.cs")))
    command_count = len(list((ROOT / "src/GrpcServer.Contracts/Commands").glob("**/*Command.cs")))
    if (endpoint_count, handler_count, command_count) != (55, 55, 55):
        fail(f"unexpected endpoint/handler/command counts: {endpoint_count}/{handler_count}/{command_count}")

    assert_contains("README.md", "27 architecture tests")
    assert_contains("README.md", "Five test projects")
    assert_contains("README.md", "Pages documentation")
    assert_absent("README.md", "github.com/jonathanperis/cpnucleo/wiki")
    assert_absent("docs/wiki/getting-started.md", "http://localhost:5300/healthz")
    assert_contains("docs/wiki/getting-started.md", "http://localhost:5301/healthz")
    assert_contains("docs/wiki/api-reference.md", "gRPC transport: `http://localhost:5300` (HTTP/2)")
    assert_contains("docs/wiki/api-reference.md", "Health check: `http://localhost:5301/healthz` (HTTP/1.1)")
    assert_contains("docs/wiki/api-reference.md", "resource-key singular envelopes")
    assert_contains("docs/wiki/webclient-crud.md", "prefilled edit forms")
    assert_contains("docs/wiki/webclient-crud.md", "readable relation labels")
    assert_contains("docs/wiki/webclient-crud.md", "singular item response normalization")
    assert_contains("docs/wiki/testing.md", "55 integration tests")
    assert_contains("docs/wiki/testing.md", "WebClient Vitest suite")
    assert_contains("docs/wiki/deployment.md", "Hostinger Docker Manager")

    print("README/wiki drift checks passed")


if __name__ == "__main__":
    main()
