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
    { label: 'Board', href: '/board/', icon: 'board' },
    { label: 'Calendar', href: '/calendar/', icon: 'calendar' },
  ] },
  { name: 'Work', items: [
    resourceItem('organizations'), resourceItem('projects'), resourceItem('assignments'), resourceItem('appointments'), resourceItem('workflows'),
  ] },
  { name: 'People', items: [resourceItem('users'), resourceItem('userAssignments'), resourceItem('userProjects')] },
  { name: 'Setup', items: [resourceItem('assignmentTypes'), resourceItem('impediments'), resourceItem('assignmentImpediments')] },
  { name: 'Status', items: [{ label: 'Service health', href: '/api-health/', icon: 'activity' }] },
];

/** Pages outside the sidebar that still belong to a group for the breadcrumb. */
const extraLocations: { href: string; group: string; label: string }[] = [
  { href: '/account/', group: 'Account', label: 'Your account' },
];

const normalizePath = (href: string) => href.replace(/\/$/, '') || '/';

export const isActivePath = (path: string, href: string) => {
  const pathname = normalizePath(path);
  const target = normalizePath(href);
  return target === '/' ? pathname === '/' : pathname === target || pathname.startsWith(`${target}/`);
};

/** The group and item for a route, used for the header breadcrumb. */
export const locate = (path: string): { group: NavigationGroup; item: NavigationItem } | undefined => {
  for (const group of navigation) {
    const item = group.items.find(candidate => isActivePath(path, candidate.href));
    if (item) return { group, item };
  }
  const extra = extraLocations.find(candidate => isActivePath(path, candidate.href));
  return extra ? { group: { name: extra.group, items: [] }, item: { label: extra.label, href: extra.href, icon: 'user' } } : undefined;
};
