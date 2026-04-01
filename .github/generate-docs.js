const fs = require('fs');
const path = require('path');
const marked = require('marked');

const DOCS_TITLE = process.env.DOCS_TITLE || 'Documentation';
const DOCS_DESCRIPTION = process.env.DOCS_DESCRIPTION || '';
const GITHUB_URL = process.env.GITHUB_URL || '';
const OUTPUT_PATH = process.env.OUTPUT_PATH || 'docs/index.html';
const TEMPLATE_PATH = process.env.TEMPLATE_PATH || path.join(__dirname, 'template.html');

const WIKI_DIR = path.join(process.cwd(), 'wiki');

function toTitleCase(str) {
  return str.split(/[-_]+/).map(word => word.charAt(0).toUpperCase() + word.slice(1)).join(' ');
}

function toSlug(str) {
  return str.toLowerCase().replace(/[-_]+/g, '-').replace(/\s+/g, '-');
}

async function generateDocs() {
  if (!fs.existsSync(WIKI_DIR)) {
    console.warn(`[WARN] Wiki directory not found at ${WIKI_DIR}`);
    return;
  }

  const files = fs.readdirSync(WIKI_DIR)
    .filter(file => file.endsWith('.md'))
    .filter(file => !['_Footer.md', '_Sidebar.md', '_Header.md'].includes(file));

  if (files.length === 0) {
    console.warn('[WARN] No markdown files found in wiki directory.');
    return;
  }

  files.sort((a, b) => {
    if (a.toLowerCase() === 'home.md') return -1;
    if (b.toLowerCase() === 'home.md') return 1;
    return a.localeCompare(b);
  });

  let mainContent = '';
  let sidebarNav = '<ul>\n';

  for (const file of files) {
    const rawName = path.basename(file, '.md');
    const title = toTitleCase(rawName);
    const slug = toSlug(rawName);
    const mdContent = fs.readFileSync(path.join(WIKI_DIR, file), 'utf-8');
    const htmlContent = marked.parse(mdContent);

    sidebarNav += `  <li><a href="#${slug}" class="nav-item">${title}</a></li>\n`;

    mainContent += `
<section id="${slug}" class="doc-section">
  <h1 class="page-title">${title}</h1>
  ${htmlContent}
</section>\n`;
  }

  sidebarNav += '</ul>';

  if (!fs.existsSync(TEMPLATE_PATH)) {
    console.error(`[ERROR] Template not found at ${TEMPLATE_PATH}`);
    process.exit(1);
  }

  let template = fs.readFileSync(TEMPLATE_PATH, 'utf-8');

  template = template.replace(/\{\{TITLE\}\}/g, DOCS_TITLE);
  template = template.replace(/\{\{DESCRIPTION\}\}/g, DOCS_DESCRIPTION);
  template = template.replace(/\{\{GITHUB_URL\}\}/g, GITHUB_URL);
  template = template.replace(/\{\{SIDEBAR_NAV\}\}/g, sidebarNav);
  template = template.replace(/\{\{MAIN_CONTENT\}\}/g, mainContent);

  const outputDir = path.dirname(OUTPUT_PATH);
  if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
  }

  fs.writeFileSync(OUTPUT_PATH, template, 'utf-8');
  console.log(`Generated docs: ${OUTPUT_PATH} (${files.length} sections from wiki)`);
}

generateDocs().catch(err => {
  console.error('[ERROR]', err);
  process.exit(1);
});