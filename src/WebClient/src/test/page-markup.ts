import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';

/** Body markup of a built page (run `astro build` first; `bun run test` does). */
export const builtPage = (route: string) => new DOMParser().parseFromString(
  readFileSync(resolve('dist', route, 'index.html'), 'utf8'), 'text/html').body.innerHTML;

/** Mounts a built page as AuthGuard would after sign-in. */
export const showBuiltPage = (route: string) => {
  document.body.innerHTML = builtPage(route);
  document.querySelector<HTMLElement>('[data-auth-content]')!.hidden = false;
};

// jsdom implements neither showModal() nor close(); mirror the browser behavior pages rely on.
export const polyfillDialogs = () => {
  HTMLDialogElement.prototype.showModal ??= function (this: HTMLDialogElement) { this.open = true; };
  HTMLDialogElement.prototype.close ??= function (this: HTMLDialogElement, value?: string) {
    if (value !== undefined) this.returnValue = value;
    this.open = false;
    this.dispatchEvent(new Event('close'));
  };
};
