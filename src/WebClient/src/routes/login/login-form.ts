export const DEFAULT_LOGIN = '';
const isHtmlFormElement = (target: unknown): target is HTMLFormElement => {
  if (!target || typeof target !== 'object') return false;
  if (typeof HTMLFormElement !== 'undefined' && target instanceof HTMLFormElement) return true;
  return 'tagName' in target && String((target as { tagName?: unknown }).tagName).toUpperCase() === 'FORM';
};

export const getLoginFormElement = (event: Event, currentTarget?: EventTarget | null): HTMLFormElement => {
  if (isHtmlFormElement(currentTarget)) return currentTarget;
  if (isHtmlFormElement(event.currentTarget)) return event.currentTarget;
  if (isHtmlFormElement(event.target)) return event.target;

  throw new TypeError('Login form submit target is unavailable.');
};
