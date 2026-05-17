#!/usr/bin/env python3
"""Generate deterministic CPnucleo social preview and icon fallback assets."""
from __future__ import annotations

import hashlib
from pathlib import Path
from typing import Iterable, Tuple

from PIL import Image, ImageDraw, ImageFont, ImageFilter

ROOT = Path(__file__).resolve().parents[1]
PUBLIC = ROOT / "docs" / "public"
FAVICON = PUBLIC / "favicon.ico"
OUT_SOCIAL = PUBLIC / "social-preview.png"
OUT_THUMB = PUBLIC / "thumbnail.png"
OUT_FAVICON_PNG = PUBLIC / "favicon.png"
OUT_FAVICON_32 = PUBLIC / "favicon-32x32.png"
OUT_APPLE = PUBLIC / "apple-touch-icon.png"

FONT_DISPLAY = Path("/usr/share/fonts/truetype/liberation/LiberationSans-Bold.ttf")
FONT_BODY = Path("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf")
FONT_BODY_BOLD = Path("/usr/share/fonts/truetype/dejavu/DejaVuSans-Bold.ttf")
FONT_MONO = Path("/usr/share/fonts/truetype/dejavu/DejaVuSansMono.ttf")
FONT_MONO_BOLD = Path("/usr/share/fonts/truetype/dejavu/DejaVuSansMono-Bold.ttf")

BG = (2, 8, 19, 255)
BG_2 = (5, 16, 31, 255)
PANEL = (8, 21, 38, 238)
PANEL_2 = (9, 30, 48, 255)
LINE = (34, 72, 98, 145)
LINE_SOFT = (18, 50, 75, 95)
CYAN = (38, 215, 242, 255)
CYAN_SOFT = (38, 215, 242, 36)
VIOLET = (148, 106, 226, 255)
TEXT = (230, 243, 255, 255)
TEXT_BODY = (163, 183, 204, 255)
MUTED = (111, 132, 153, 255)
SUCCESS = (96, 210, 157, 255)


def font(path: Path, size: int) -> ImageFont.FreeTypeFont:
    """Load a TrueType font at the requested pixel size."""
    return ImageFont.truetype(str(path), size=size)


def alpha(color: Tuple[int, int, int, int], a: int) -> Tuple[int, int, int, int]:
    """Return a copy of an RGBA color with a replaced alpha channel."""
    return color[:3] + (a,)


def text_size(draw: ImageDraw.ImageDraw, text: str, fnt: ImageFont.ImageFont) -> Tuple[int, int]:
    """Measure text with Pillow's bounding-box API."""
    box = draw.textbbox((0, 0), text, font=fnt)
    return box[2] - box[0], box[3] - box[1]


def rounded(draw: ImageDraw.ImageDraw, xy, radius: int, fill, outline=None, width: int = 1):
    """Draw a rounded rectangle using the shared CPnucleo shape language."""
    draw.rounded_rectangle(xy, radius=radius, fill=fill, outline=outline, width=width)


def draw_grid(draw: ImageDraw.ImageDraw, w: int, h: int) -> None:
    """Paint the subtle blueprint grid used by the Pages surface."""
    for step, col, width in [(42, (24, 75, 103, 42), 1), (168, (38, 215, 242, 32), 1)]:
        for x in range(-step, w + step, step):
            draw.line((x, 0, x, h), fill=col, width=width)
        for y in range(-step, h + step, step):
            draw.line((0, y, w, y), fill=col, width=width)


def draw_glow(base: Image.Image, boxes: Iterable[Tuple[Tuple[int, int, int, int], Tuple[int, int, int, int], int]]) -> None:
    """Composite a restrained cyan glow behind important rounded panels."""
    glow = Image.new("RGBA", base.size, (0, 0, 0, 0))
    gd = ImageDraw.Draw(glow)
    for xy, col, radius in boxes:
        gd.rounded_rectangle(xy, radius=radius, outline=col, width=4)
    glow = glow.filter(ImageFilter.GaussianBlur(16))
    base.alpha_composite(glow)


def load_icon(size: int) -> Image.Image:
    """Load the largest favicon frame and resize it for the requested asset."""
    icon = Image.open(FAVICON).convert("RGBA")
    return icon.resize((size, size), Image.Resampling.LANCZOS)


def draw_badge(draw: ImageDraw.ImageDraw, xy, label: str, tone=CYAN) -> int:
    """Draw a compact proof badge and return its right edge for layout flow."""
    f = font(FONT_MONO_BOLD, 17)
    pad_x, pad_y = 13, 7
    tw, th = text_size(draw, label, f)
    x, y = xy
    rounded(draw, (x, y, x + tw + pad_x * 2, y + th + pad_y * 2), 999, fill=(5, 18, 32, 245), outline=alpha(tone, 185), width=1)
    draw.text((x + pad_x, y + pad_y - 1), label, font=f, fill=tone)
    return x + tw + pad_x * 2


def draw_node(draw: ImageDraw.ImageDraw, xy, label: str, sub: str | None = None, accent=CYAN) -> None:
    """Draw one service node inside the topology panel."""
    x1, y1, x2, y2 = xy
    rounded(draw, xy, 18, fill=(8, 24, 42, 245), outline=alpha(accent, 175), width=2)
    draw.ellipse((x1 + 18, y1 + 22, x1 + 30, y1 + 34), fill=accent)
    draw.text((x1 + 42, y1 + 17), label, font=font(FONT_BODY_BOLD, 22), fill=TEXT)
    if sub:
        draw.text((x1 + 42, y1 + 47), sub, font=font(FONT_MONO, 14), fill=MUTED)


def draw_topology(draw: ImageDraw.ImageDraw) -> None:
    """Draw the right-side service topology motif from the live Pages design."""
    panel = (728, 94, 1116, 542)
    rounded(draw, panel, 28, fill=PANEL, outline=(36, 97, 128, 170), width=1)
    draw.text((758, 122), "LIVE TOPOLOGY", font=font(FONT_MONO_BOLD, 18), fill=CYAN)
    draw.text((758, 150), "4 SERVICES · 2 TRANSPORTS", font=font(FONT_MONO, 13), fill=MUTED)

    cx = 922
    # connector lines
    for line in [
        (806, 229, 806, 279), (1038, 229, 1038, 279),
        (806, 360, 806, 405), (1038, 360, 1038, 405),
        (806, 279, 1038, 279), (806, 405, 1038, 405),
        (922, 279, 922, 405),
    ]:
        draw.line(line, fill=alpha(CYAN, 95), width=2)

    draw_node(draw, (758, 190, 922, 262), "WebClient", "Blazor", CYAN)
    draw_node(draw, (944, 190, 1086, 262), "WebApi", "REST", CYAN)
    draw_node(draw, (758, 407, 922, 479), "Identity", "JWT", VIOLET)
    draw_node(draw, (944, 407, 1086, 479), "GrpcServer", "Dapper", VIOLET)

    rounded(draw, (792, 296, 1052, 370), 24, fill=(16, 54, 74, 255), outline=alpha(CYAN, 210), width=2)
    draw.text((838, 315), "Domain core", font=font(FONT_BODY_BOLD, 28), fill=TEXT)
    draw.text((839, 347), "CQRS · DDD · tests", font=font(FONT_MONO, 15), fill=TEXT_BODY)

    draw.text((771, 502), "PostgreSQL · NGINX · OpenTelemetry", font=font(FONT_MONO, 14), fill=TEXT_BODY)


def draw_social(path: Path, size=(1200, 630), scale: float = 1.0) -> None:
    """Render the primary Open Graph social preview image."""
    w, h = size
    img = Image.new("RGBA", size, BG)
    draw = ImageDraw.Draw(img)
    # subtle radial-ish slabs
    for i in range(0, h, 3):
        t = i / max(1, h - 1)
        r = int(BG[0] * (1 - t) + BG_2[0] * t)
        g = int(BG[1] * (1 - t) + BG_2[1] * t)
        b = int(BG[2] * (1 - t) + BG_2[2] * t)
        draw.line((0, i, w, i), fill=(r, g, b, 255), width=3)
    draw_grid(draw, w, h)
    draw_glow(img, [((728, 94, 1116, 542), alpha(CYAN, 70), 28), ((62, 54, 168, 160), alpha(CYAN, 95), 28)])
    draw = ImageDraw.Draw(img)

    # brand mark
    icon = load_icon(106)
    img.alpha_composite(icon, (62, 54))
    draw.text((184, 67), "CPNUCLEO", font=font(FONT_DISPLAY, 34), fill=TEXT)
    draw.text((186, 105), ".NET ARCHITECTURE WORKBENCH", font=font(FONT_MONO_BOLD, 17), fill=CYAN)

    # eyebrow and headline
    draw.text((66, 205), "CLEAN ARCHITECTURE · REST · GRPC · BLAZOR", font=font(FONT_MONO_BOLD, 19), fill=CYAN)
    head = font(FONT_DISPLAY, 58)
    y = 244
    for line in ["CLEAN ARCHITECTURE", "REFERENCE", "SYSTEM FOR .NET 10"]:
        draw.text((64, y), line, font=head, fill=TEXT)
        y += 68
    draw.text((68, 478), "Source-backed docs for service boundaries,", font=font(FONT_BODY, 23), fill=TEXT_BODY)
    draw.text((68, 510), "release paths, Azure deploys, and tests.", font=font(FONT_BODY, 23), fill=TEXT_BODY)

    # badges
    x = 68
    y = 562
    for label, tone in [("FastEndpoints", CYAN), ("PostgreSQL", SUCCESS), ("Docker", VIOLET), ("Azure", CYAN)]:
        next_x = draw_badge(draw, (x, y), label, tone)
        x = next_x + 12

    draw_topology(draw)

    # frame
    rounded(draw, (24, 24, w - 24, h - 24), 34, fill=None, outline=(37, 91, 119, 120), width=1)
    img.convert("RGB").save(path, optimize=True)


def make_icons() -> None:
    """Write PNG favicon fallbacks from the canonical ICO source."""
    icon256 = load_icon(256)
    icon256.save(OUT_FAVICON_PNG, optimize=True)
    icon256.resize((32, 32), Image.Resampling.LANCZOS).save(OUT_FAVICON_32, optimize=True)
    icon256.resize((180, 180), Image.Resampling.LANCZOS).save(OUT_APPLE, optimize=True)


def assert_assets() -> None:
    """Verify generated image dimensions and formats before committing."""
    expected = {
        OUT_SOCIAL: (1200, 630),
        OUT_THUMB: (600, 315),
        OUT_FAVICON_PNG: (256, 256),
        OUT_FAVICON_32: (32, 32),
        OUT_APPLE: (180, 180),
    }
    for path, size in expected.items():
        im = Image.open(path)
        assert im.size == size, f"{path} expected {size}, got {im.size}"
        assert im.format == "PNG", f"{path} expected PNG, got {im.format}"
        assert path.stat().st_size > 1000, f"{path} unexpectedly small"
    ico = Image.open(FAVICON)
    assert (256, 256) in ico.ico.sizes(), "favicon.ico lacks 256px frame"


def digest(paths: Iterable[Path]) -> str:
    """Return a stable digest for deterministic asset checks."""
    h = hashlib.sha256()
    for path in paths:
        h.update(path.name.encode())
        h.update(path.read_bytes())
    return h.hexdigest()


def main() -> None:
    """Generate every CPnucleo social preview and icon fallback asset."""
    PUBLIC.mkdir(parents=True, exist_ok=True)
    make_icons()
    draw_social(OUT_SOCIAL, (1200, 630))
    # Downsample from primary preview to guarantee exact sibling composition at compact card scale.
    Image.open(OUT_SOCIAL).resize((600, 315), Image.Resampling.LANCZOS).save(OUT_THUMB, optimize=True)
    assert_assets()
    paths = [OUT_SOCIAL, OUT_THUMB, OUT_FAVICON_PNG, OUT_FAVICON_32, OUT_APPLE]
    print("generated")
    for path in paths:
        im = Image.open(path)
        print(f"{path.relative_to(ROOT)} {im.size} {path.stat().st_size} bytes")
    print("sha256", digest(paths))


if __name__ == "__main__":
    main()
