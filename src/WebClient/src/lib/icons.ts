import type { ResourceKey } from '~/lib/api/types';

/**
 * Stroke icons on a 24px grid, stored as path data so Astro templates and the TypeScript
 * controllers render the same glyphs without parsing markup.
 */
export const icons = {
  home: ['M3 10.5 12 3l9 7.5', 'M5 9.5V21h14V9.5', 'M10 21v-6h4v6'],
  building: ['M4 21V5a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v16', 'M16 9h2a2 2 0 0 1 2 2v10', 'M3 21h18', 'M8 7h4', 'M8 11h4', 'M8 15h4'],
  folder: ['M3 7a2 2 0 0 1 2-2h4l2 2h8a2 2 0 0 1 2 2v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2Z'],
  task: ['M4 5a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1Z', 'm8.5 12 2.5 2.5 5-5'],
  steps: ['M6 4v12', 'M12 4v7', 'M18 4v15'],
  calendar: ['M4 7a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2Z', 'M4 10h16', 'M8 3v4', 'M16 3v4'],
  users: ['M16 20v-1.5a3.5 3.5 0 0 0-3.5-3.5h-5A3.5 3.5 0 0 0 4 18.5V20', 'M13.5 8a3.5 3.5 0 1 1-7 0a3.5 3.5 0 1 1 7 0', 'M20 20v-1.5a3.5 3.5 0 0 0-2.5-3.35', 'M15.5 4.65a3.5 3.5 0 0 1 0 6.7'],
  userCheck: ['M14 20v-1.5a3.5 3.5 0 0 0-3.5-3.5h-4A3.5 3.5 0 0 0 3 18.5V20', 'M12 8a3.5 3.5 0 1 1-7 0a3.5 3.5 0 1 1 7 0', 'm16 11 2 2 4-4'],
  userPlus: ['M14 20v-1.5a3.5 3.5 0 0 0-3.5-3.5h-4A3.5 3.5 0 0 0 3 18.5V20', 'M12 8a3.5 3.5 0 1 1-7 0a3.5 3.5 0 1 1 7 0', 'M19 8v6', 'M16 11h6'],
  tag: ['M3 12V4a1 1 0 0 1 1-1h8l9 9-9 9Z', 'M7.5 7.5h.01'],
  blocker: ['M8 3h8l5 5v8l-5 5H8l-5-5V8Z', 'M12 8v5', 'M12 16.5h.01'],
  link: ['M10 14a4 4 0 0 0 5.66 0l3-3a4 4 0 0 0-5.66-5.66l-1 1', 'M14 10a4 4 0 0 0-5.66 0l-3 3a4 4 0 0 0 5.66 5.66l1-1'],
  activity: ['M3 12h4l3-8 4 16 3-8h4'],
  plus: ['M12 5v14', 'M5 12h14'],
  pencil: ['M4 20h4L19 9a2.83 2.83 0 0 0-4-4L4 16Z', 'm13.5 6.5 4 4'],
  trash: ['M4 7h16', 'M10 11v6', 'M14 11v6', 'M6 7l1 12a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2l1-12', 'M9 7V4h6v3'],
  eye: ['M2.5 12S6 5 12 5s9.5 7 9.5 7-3.5 7-9.5 7S2.5 12 2.5 12Z', 'M15 12a3 3 0 1 1-6 0a3 3 0 1 1 6 0'],
  refresh: ['M20 11a8 8 0 0 0-14.9-3', 'M4 4v4h4', 'M4 13a8 8 0 0 0 14.9 3', 'M20 20v-4h-4'],
  chevronLeft: ['m15 18-6-6 6-6'],
  chevronRight: ['m9 18 6-6-6-6'],
  search: ['M17 11a6 6 0 1 1-12 0a6 6 0 1 1 12 0', 'm20 20-4.35-4.35'],
  sun: ['M16 12a4 4 0 1 1-8 0a4 4 0 1 1 8 0', 'M12 2v2', 'M12 20v2', 'm4.93 4.93 1.41 1.41', 'm17.66 17.66 1.41 1.41', 'M2 12h2', 'M20 12h2', 'm6.34 17.66-1.41 1.41', 'm19.07 4.93-1.41 1.41'],
  moon: ['M20.5 14.5A8.5 8.5 0 1 1 9.5 3.5a7 7 0 0 0 11 11Z'],
  logOut: ['M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4', 'm16 17 5-5-5-5', 'M21 12H9'],
  close: ['M18 6 6 18', 'm6 6 12 12'],
  menu: ['M4 6h16', 'M4 12h16', 'M4 18h16'],
  arrowRight: ['M5 12h14', 'm13 6 6 6-6 6'],
  external: ['M14 4h6v6', 'M20 4 10 14', 'M18 14v5a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V7a1 1 0 0 1 1-1h5'],
  shield: ['M12 3 5 6v5c0 4.5 3 8.5 7 10 4-1.5 7-5.5 7-10V6Z', 'm9 12 2 2 4-4'],
  book: ['M4 19V5a2 2 0 0 1 2-2h14v14H6a2 2 0 0 0-2 2Zm0 0a2 2 0 0 0 2 2h14'],
  server: ['M4 4h16v6H4Z', 'M4 14h16v6H4Z', 'M8 7h.01', 'M8 17h.01'],
  database: ['M4 6c0-1.66 3.58-3 8-3s8 1.34 8 3-3.58 3-8 3-8-1.34-8-3Z', 'M4 6v12c0 1.66 3.58 3 8 3s8-1.34 8-3V6', 'M4 12c0 1.66 3.58 3 8 3s8-1.34 8-3'],
  key: ['M11 15a4 4 0 1 1-8 0a4 4 0 1 1 8 0', 'm10 12 9-9', 'm16 6 3 3', 'm14 8 2 2'],
  alert: ['M21 12a9 9 0 1 1-18 0a9 9 0 1 1 18 0', 'M12 8v4.5', 'M12 16h.01'],
  check: ['m5 12 5 5 9-10'],
  board: ['M4 5a1 1 0 0 1 1-1h3a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1Z', 'M10.5 5a1 1 0 0 1 1-1h3a1 1 0 0 1 1 1v9a1 1 0 0 1-1 1h-3a1 1 0 0 1-1-1Z', 'M17 5a1 1 0 0 1 1-1h1a1 1 0 0 1 1 1v6a1 1 0 0 1-1 1h-1a1 1 0 0 1-1-1Z'],
  user: ['M18 20v-1.5a3.5 3.5 0 0 0-3.5-3.5h-5A3.5 3.5 0 0 0 6 18.5V20', 'M15.5 8a3.5 3.5 0 1 1-7 0a3.5 3.5 0 1 1 7 0'],
  columns: ['M4 5a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v14a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1Z', 'M9.5 4v16', 'M14.5 4v16'],
  sortAsc: ['m8 9 4-4 4 4', 'M12 5v14'],
  sortDesc: ['m8 15 4 4 4-4', 'M12 19V5'],
  sort: ['m8 9 4-4 4 4', 'm8 15 4 4 4-4'],
  filter: ['M4 5h16l-6 7.5V19l-4 1.5v-8Z'],
  undo: ['M9 14 4 9l5-5', 'M4 9h10.5a5.5 5.5 0 0 1 0 11H11'],
  clock: ['M21 12a9 9 0 1 1-18 0a9 9 0 1 1 18 0', 'M12 7v5l3 2'],
  monitor: ['M3 5a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1Z', 'M8 20h8', 'M12 16v4'],
  command: ['M9 6a3 3 0 1 0-3 3h12a3 3 0 1 0-3-3v12a3 3 0 1 0 3-3H6a3 3 0 1 0 3 3Z'],
  network: ['m7 4-4 4 4 4', 'M3 8h13', 'm17 12 4 4-4 4', 'M21 16H8'],
} as const satisfies Record<string, readonly string[]>;

export type IconName = keyof typeof icons;

export const resourceIcons: Record<ResourceKey, IconName> = {
  organizations: 'building',
  projects: 'folder',
  assignments: 'task',
  workflows: 'steps',
  appointments: 'calendar',
  users: 'users',
  userAssignments: 'userCheck',
  userProjects: 'userPlus',
  assignmentTypes: 'tag',
  impediments: 'blocker',
  assignmentImpediments: 'link',
};

const SVG_NS = 'http://www.w3.org/2000/svg';

/** Builds a decorative icon element; the surrounding control carries the accessible name. */
export const createIcon = (name: IconName, className = 'size-4'): SVGSVGElement => {
  const svg = document.createElementNS(SVG_NS, 'svg');
  for (const [attribute, value] of Object.entries({
    viewBox: '0 0 24 24', fill: 'none', stroke: 'currentColor', 'stroke-width': '1.75',
    'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'aria-hidden': 'true', class: className,
  })) svg.setAttribute(attribute, value);
  for (const d of icons[name]) {
    const path = document.createElementNS(SVG_NS, 'path');
    path.setAttribute('d', d);
    svg.append(path);
  }
  return svg;
};
