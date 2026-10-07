import { completeSignIn } from '~/lib/api/oidc';

/**
 * The redirect URI of the authorization code flow: redeems the code and opens the page the
 * sign-in started from. The code is removed from the address bar and history first.
 */
export const mountSignInCallback = async (
  section: HTMLElement,
  location: Pick<Location, 'search'> = window.location,
  replace: (url: string) => void = url => window.location.replace(url),
): Promise<void> => {
  const search = location.search;
  window.history.replaceState(null, '', '/signin-callback/');
  try {
    replace(await completeSignIn(search));
  } catch (failure) {
    section.querySelector<HTMLElement>('[data-callback-status]')!.textContent = '';
    const error = section.querySelector<HTMLElement>('[data-callback-error]')!;
    error.textContent = failure instanceof Error ? failure.message : 'The sign-in could not be completed.';
    error.hidden = false;
    section.querySelector<HTMLElement>('[data-callback-retry]')!.hidden = false;
  }
};
