#!/usr/bin/env python3
"""Check current public documentation contracts without freezing test-case totals."""
import json
import re
import argparse
from html.parser import HTMLParser
from pathlib import Path
from urllib.parse import unquote, urljoin, urlsplit

ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8")


def require(condition, message):
    if not condition:
        raise SystemExit(f"docs drift: {message}")


class PageLinks(HTMLParser):
    def __init__(self, content):
        super().__init__()
        self.links = []
        self.ids = set()
        self.feed(content)

    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        if attrs.get("id"):
            self.ids.add(attrs["id"])
        for name in ("href", "src"):
            if attrs.get(name):
                self.links.append(attrs[name])


def check_built_site(output, site_url="https://jonathanperis.github.io/cpnucleo/"):
    pages = {path: PageLinks(path.read_text(encoding="utf-8")) for path in output.rglob("*.html")}
    require(output / "index.html" in pages, "build docs before checking generated links")
    site = urlsplit(site_url)
    for path, page in pages.items():
        relative = path.relative_to(output).as_posix()
        page_url = urljoin(site_url, relative.removesuffix("index.html"))
        for link in page.links:
            target = urlsplit(urljoin(page_url, link))
            if target.netloc != site.netloc or target.scheme not in ("http", "https"):
                continue
            if target.path != site.path.rstrip("/") and not target.path.startswith(site.path):
                require(bool(urlsplit(link).netloc), f"{relative}: link escapes the Pages base: {link}")
                continue  # Absolute links to the author's other GitHub Pages sites.
            destination = output / unquote(target.path[len(site.path):])
            if destination.is_dir():
                destination /= "index.html"
            require(destination.is_file(), f"{relative}: missing local target: {link}")
            if target.fragment and destination in pages:
                require(unquote(target.fragment) in pages[destination].ids,
                        f"{relative}: missing fragment: {link}")
    print(f"Generated documentation links/assets passed: {len(pages)} HTML pages.")


# Owner requirement: every page in both Astro sites is an .astro page and no client UI framework
# is installed. Mirrored by src/WebClient/src/guarantees/astro-pages.test.ts and
# tests/Architecture.Tests/AstroPagesTests.cs.
ASTRO_SITES = {"WebClient": "src/WebClient", "docs": "docs"}
FORBIDDEN_FRAMEWORK_PACKAGES = {
    "react", "react-dom", "preact", "vue", "petite-vue", "svelte",
    "solid-js", "lit", "lit-html", "alpinejs", "htmx.org", "@angular/core",
    "@stencil/core", "@builder.io/qwik", "@builder.io/qwik-city", "@qwik.dev/core", "@qwik.dev/router", "@qwikdev/astro",
    "@astrojs/react", "@astrojs/vue", "@astrojs/svelte", "@astrojs/solid-js", "@astrojs/preact", "@astrojs/lit",
    "@astrojs/alpinejs", "@analogjs/astro-angular",
}
ASTRO_GENERATOR = re.compile(r'<meta name="generator" content="Astro v\d')


def check_astro_site(site, label):
    pages = [path for path in (site / "src/pages").rglob("*") if path.is_file()]
    require(pages, f"{label}: no pages found under src/pages")
    for path in pages:
        require(path.suffix == ".astro", f"{label}: non-Astro page {path.relative_to(site).as_posix()}")
    public = site / "public"
    for path in (public.rglob("*") if public.is_dir() else []):
        require(path.suffix.lower() not in (".html", ".htm"),
                f"{label}: hand-written HTML page served outside Astro: {path.relative_to(site).as_posix()}")
    package = json.loads((site / "package.json").read_text(encoding="utf-8"))
    names = {name for section in ("dependencies", "devDependencies", "peerDependencies",
                                  "optionalDependencies", "overrides")
             for name in package.get(section, {})}
    # npm aliases ("ui": "npm:react@19") hide the real package behind another name.
    aliased = {match.group(1)
               for section in ("dependencies", "devDependencies", "optionalDependencies")
               for value in package.get(section, {}).values()
               if isinstance(value, str) and (match := re.match(r"npm:((?:@[^/@]+/)?[^@]+)", value))}
    forbidden = sorted((names | aliased) & FORBIDDEN_FRAMEWORK_PACKAGES)
    require(not forbidden, f"{label}: client UI framework dependency {forbidden}")
    lockfile = site / "bun.lock"
    locked = sorted(name for name in FORBIDDEN_FRAMEWORK_PACKAGES
                    if lockfile.is_file() and f'"{name}@' in lockfile.read_text(encoding="utf-8"))
    require(not locked, f"{label}: client UI framework installed transitively {locked} (bun.lock)")
    config = (site / "astro.config.mjs").read_text(encoding="utf-8")
    imported = sorted(name for name in FORBIDDEN_FRAMEWORK_PACKAGES
                      if f"'{name}'" in config or f'"{name}"' in config)
    require(not imported, f"{label}: client UI framework integration {imported} in astro.config.mjs")
    return len(pages)


def check_astro_output(output, label):
    pages = sorted(output.rglob("*.html")) if output.is_dir() else []
    require(pages, f"{label}: build output {output} has no HTML pages")
    for path in pages:
        html = path.read_text(encoding="utf-8")
        name = path.relative_to(output).as_posix()
        require(ASTRO_GENERATOR.search(html), f"{label}: {name} was not generated by an Astro page")
        # Astro alone never hydrates components; islands mean a client framework renderer slipped in.
        require("<astro-island" not in html, f"{label}: {name} hydrates a client component island")
        foreign = [src for src in re.findall(r'<script[^>]*\ssrc="([^"]+)"', html) if "/_astro/" not in src]
        require(not foreign, f"{label}: {name} loads scripts Astro did not bundle: {foreign}")
    return len(pages)


def main(built_site=False):
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
    astro_pages = {label: check_astro_site(ROOT / path, label) for label, path in ASTRO_SITES.items()}

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
        for target in re.findall(r"\]\(([a-z0-9-]+)\)", path.read_text(encoding="utf-8")):
            require(target not in slugs, f"{slug}: link to {target} must use ../{target}/")

    api = read("docs/wiki/api-reference.md")
    rows = re.findall(r"\| (\w+) \| `(/api/\w+)` \| `(/api/\w+)` \|", api)
    resources = ROOT / "src/WebApi/Endpoints"
    require({row[0] for row in rows} == {path.name for path in resources.iterdir() if path.is_dir()},
            "API resource table differs from endpoint resources")
    for entity, singular, plural in rows:
        routes = set()
        for endpoint in (resources / entity).glob("**/Endpoint.cs"):
            routes.update((method.upper(), "/api" + route) for method, route in
                          re.findall(r'\b(Get|Post|Patch|Put|Delete)\("([^"]+)"\)', endpoint.read_text()))
        expected = {("POST", singular), ("GET", singular), ("GET", plural),
                    ("PATCH", singular), ("DELETE", singular)}
        require(routes == expected, f"{entity}: documented CRUD verbs/routes differ: {routes}")

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
    print("Astro-only pages passed: " + ", ".join(f"{label} {count} source page(s)" for label, count in astro_pages.items()) + ".")
    if built_site:
        output = ROOT / "docs/out"
        for slug in slugs:
            require((output / "docs" / slug / "index.html").is_file(), f"{slug} is not published")
        check_built_site(output)
        print(f"Docs build output is Astro-generated: {check_astro_output(output, 'docs')} HTML pages.")
        web_client_output = ROOT / "src/WebClient/dist"
        if web_client_output.is_dir():
            count = check_astro_output(web_client_output, "WebClient")
            print(f"WebClient build output is Astro-generated: {count} HTML pages.")
        else:
            print("WebClient dist/ is not built here; its `bun run test` asserts the Astro output.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--built-site", action="store_true", help="also validate docs/out links after building")
    main(parser.parse_args().built_site)
