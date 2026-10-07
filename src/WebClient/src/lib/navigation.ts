import { findResource } from '~/lib/api/resource-metadata';
import type { ResourceKey } from '~/lib/api/types';
import { resourceIcons, type IconName } from '~/lib/icons';

export interface NavigationItem {
  label: string;
  href: string;
  icon: IconName;
  resource?: ResourceKey;
}

export interface NavigationGroup {
  name: string;
  items: NavigationItem[];
}

const resourceItem = (key: ResourceKey, label?: string): NavigationItem => {
  const resource = findResource(key);
  return { label: label ?? resource.pluralLabel, href: resource.routePath, icon: resourceIcons[key], resource: key };
};

/** Sidebar, mobile menu, breadcrumbs and the home overview share this grouping. */
export const navigation: NavigationGroup[] = [
  { name: 'Start', items: [
    { label: 'Home', href: '/', icon: 'home' },
    resourceItem('organizations'), resourceItem('projects'), resourceItem('assignments'), resourceItem('workflows'), resourceItem('appointments', 'Calendar'),
  ] },
  { name: 'People', items: [resourceItem('users'), resourceItem('userAssignments'), resourceItem('userProjects')] },
  { name: 'Setup', items: [resourceItem('assignmentTypes'), resourceItem('impediments'), resourceItem('assignmentImpediments')] },
  { name: 'Status', items: [{ label: 'Service health', href: '/api-health/', icon: 'activity' }] },
];

const normalizePath = (href: string) => href.replace(/\/$/, '') || '/';

export const isActivePath = (path: string, href: string) => {
  const pathname = normalizePath(path);
  const target = normalizePath(href);
  return target === '/' ? pathname === '/' : pathname === target || pathname.startsWith(`${target}/`);
};

/** The group and item for a route, used for the header breadcrumb. */
export const locate = (path: string) => {
  for (const group of navigation) {
    const item = group.items.find(candidate => isActivePath(path, candidate.href));
    if (item) return { group, item };
  }
  return undefined;
};
