"""Regression proof for the Pages-base and generated-link audit; no browser required."""
from runpy import run_path
import tempfile
import unittest
from pathlib import Path

check_built_site = run_path(str(Path(__file__).with_name("check-docs-drift.py")))["check_built_site"]


class GeneratedLinksTests(unittest.TestCase):
    def test_pages_base_relative_links_assets_and_fragments(self):
        # Fixtures are task-owned and stay inside the repository workspace.
        with tempfile.TemporaryDirectory(dir=Path(__file__).parent) as directory:
            root = Path(directory)
            article = root / "docs" / "article"
            article.mkdir(parents=True)
            (root / "asset.css").write_text("body {}")
            (root / "index.html").write_text('<a href="/cpnucleo/docs/article/#section">Read</a>')
            article_file = article / "index.html"
            valid = '<h1 id="section">Title</h1><a href="../../">Home</a><a href="/cpnucleo">Base</a><link href="../../asset.css"><a href="https://example.org/">External</a>'
            article_file.write_text(valid)
            check_built_site(root)
            for link, message in [
                ("../missing/", "missing local target"),
                ("#absent", "missing fragment"),
                ("/docs/article/", "escapes the Pages base"),
                ("../../../missing/", "escapes the Pages base"),
                ("../../missing.png", "missing local target"),
            ]:
                with self.subTest(link=link):
                    article_file.write_text(valid + f'<a href="{link}">Broken</a>')
                    with self.assertRaisesRegex(SystemExit, message):
                        check_built_site(root)


if __name__ == "__main__":
    unittest.main()
