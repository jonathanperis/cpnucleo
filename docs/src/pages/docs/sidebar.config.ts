export const SECTION_CATEGORIES = [
  { label: "", ids: ["home"] },
  { label: "Overview", ids: ["architecture", "api-reference", "database"] },
  { label: "Develop", ids: ["getting-started", "project-structure", "technologies", "testing", "deployment"] },
] as const;

export const SECTION_ORDER = SECTION_CATEGORIES.flatMap(({ ids }) => ids);
