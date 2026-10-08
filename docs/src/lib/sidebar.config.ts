export const SLUG_LABEL: Record<string, string> = {
  home: 'Project Overview',
  'learning-lab': 'Learning Lab',
  architecture: 'Architecture',
  'api-reference': 'API Reference',
  'webclient-crud': 'WebClient CRUD',
  database: 'Database',
  'getting-started': 'Getting Started',
  'project-structure': 'Project Structure',
  technologies: 'Technologies',
  testing: 'Testing',
  deployment: 'Deployment',
};

export const DOC_SUMMARIES: Record<string, string> = {
  home: 'Project overview, maturity levels and repository entry points.',
  'learning-lab': 'Guided exercises, verified examples, experiments, benchmarks and recovery.',
  architecture: 'Layer boundaries, persistence comparisons and the service split.',
  'api-reference': 'REST endpoints, the OpenID Connect provider and gRPC command contracts.',
  'webclient-crud': 'Astro and native TypeScript forms, relation search, concurrency and live updates.',
  database: 'PostgreSQL, EF Core, Dapper, migrations, integrity triggers and seed tools.',
  'getting-started': 'Prerequisites, the minimal Docker lab, seed data and source development.',
  'project-structure': 'Repository tree: source projects, tests, labs and documentation.',
  technologies: 'Runtime, libraries, infrastructure, observability and where versions live.',
  testing: 'What each test suite proves and how to run it.',
  deployment: 'Images, GitHub Actions, NGINX, Hostinger and the release gates.',
};

export const SECTION_CATEGORIES = [
  { label: "Start", ids: ["home", "learning-lab"] },
  { label: "Overview", ids: ["architecture", "api-reference", "webclient-crud", "database"] },
  { label: "Develop", ids: ["getting-started", "project-structure", "technologies", "testing", "deployment"] },
] as const;

export const SECTION_ORDER: readonly string[] = SECTION_CATEGORIES.flatMap(({ ids }) => ids);

/** Two-digit chapter number shown in the contents and running heads ("01", "02", ...). */
export const chapterNumber = (slug: string) => String(SECTION_ORDER.indexOf(slug) + 1).padStart(2, '0');
