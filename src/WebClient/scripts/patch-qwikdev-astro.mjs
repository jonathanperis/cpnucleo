import { readFileSync, writeFileSync } from 'node:fs';
import { join } from 'node:path';

const integrationPath = join(process.cwd(), 'node_modules', '@qwikdev', 'astro', 'src', 'index.ts');
let source = readFileSync(integrationPath, 'utf8');

source = source.replace(
  'const files = fs.readdirSync(serverChunksDir);',
  'const files = fs.readdirSync(serverChunksDir, { recursive: true }) as string[];'
);
source = source.replace(
  '(f) => f.startsWith("server_") && f.endsWith(".mjs")',
  '(f) => f.includes("server_") && f.endsWith(".mjs")'
);
source = source.replace(
  'const newContent = content.replace(\n                  "serverData: props,",\n                  `serverData: props, manifest: ${manifestJson},`\n                );\n\n                fs.writeFileSync(serverPath, newContent);',
  'let newContent = content.replace(\n                  "serverData: props,",\n                  `serverData: props, manifest: ${manifestJson},`\n                );\n                newContent = newContent.replace(\n                  /import \\{ A as ASTRO_VERSION, ([^}]+) \\} from \'([^\']*encryption[^\']*)\';/,\n                  (_match, imports, importPath) => `import { ${imports} } from \'${importPath}\';`\n                );\n                newContent = newContent.replace(/generator: `Astro v\\$\\{ASTRO_VERSION\\}`/g, \'generator: "Astro"\');\n\n                fs.writeFileSync(serverPath, newContent);'
);

writeFileSync(integrationPath, source);
console.log('Patched @qwikdev/astro static build server chunk handling.');
