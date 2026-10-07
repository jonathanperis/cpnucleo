#!/usr/bin/env bash
# Installs a pinned cosign release for the runner's architecture and verifies its SHA-256 before
# use. The repository's Actions policy allows only GitHub-owned and explicitly listed actions, so
# CI installs tools with checksum-pinned downloads instead of third-party installer actions.
set -Eeuo pipefail

readonly version="v3.1.3"
case "$(uname -m)" in
  x86_64) arch=amd64 sha256=4629c757b7618056f8ddd7e2625ae9fdd94c0372a65049520bc7d9df9efc7f71 ;;
  aarch64 | arm64) arch=arm64 sha256=c5d324e091826b0d7a78eb16fef316450b4eb9aaec045611c08ba06f5e73220a ;;
  *) echo "Unsupported architecture: $(uname -m)" >&2; exit 1 ;;
esac

bin_dir="${RUNNER_TEMP:?RUNNER_TEMP is required}/cosign-bin"
mkdir -p "${bin_dir}"
curl -sSfL --retry 3 -o "${bin_dir}/cosign" \
  "https://github.com/sigstore/cosign/releases/download/${version}/cosign-linux-${arch}"
echo "${sha256}  ${bin_dir}/cosign" | sha256sum --check --strict -
chmod 0755 "${bin_dir}/cosign"
echo "${bin_dir}" >> "${GITHUB_PATH:?GITHUB_PATH is required}"
"${bin_dir}/cosign" version
