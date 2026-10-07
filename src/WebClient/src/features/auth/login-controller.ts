import { accountLoginUrl, buildAuthorizeUrl, describeSignInError } from '~/lib/api/oidc';

/** Set by the account page when a password change ended the session; shown once at sign-in. */
export const signInNoticeStorageKey = 'cpnucleo.signInNotice';

const formatSeconds = (seconds: number) => {
  if (seconds < 60) return `${seconds} second${seconds === 1 ? '' : 's'}`;
  const minutes = Math.ceil(seconds / 60);
  return `${minutes} minute${minutes === 1 ? '' : 's'}`;
};

/**
 * The sign-in page of the authorization code flow:
 * - opened by the identity server (`authRequest`): shows the native form, which posts to
 *   IdentityApi; errors come back as `error` codes;
 * - opened directly (`returnUrl`, or `state` after an end-session): starts a sign-in;
 * - after a sign-out in another tab (`signedOut`) or a rejected request: explains and waits.
 */
export const mountLoginPage = async (
  section: HTMLElement,
  location: Pick<Location, 'search'> = window.location,
  navigate: (url: string) => void = url => window.location.assign(url),
): Promise<void> => {
  const form = section.querySelector<HTMLFormElement>('[data-login-form]')!;
  const error = section.querySelector<HTMLElement>('[data-login-error]')!;
  const status = section.querySelector<HTMLElement>('[data-login-status]')!;
  const restart = section.querySelector<HTMLButtonElement>('[data-login-restart]')!;
  const button = form.querySelector<HTMLButtonElement>('[type="submit"]')!;
  const parameters = new URLSearchParams(location.search);
  const returnUrl = parameters.get('returnUrl') ?? parameters.get('state');

  const showError = (message: string) => { error.textContent = message; error.hidden = false; };
  const begin = async () => {
    status.textContent = 'Opening the sign-in…';
    try {
      navigate(await buildAuthorizeUrl(returnUrl));
    } catch {
      status.textContent = '';
      showError('The sign-in could not start. Try again.');
      restart.hidden = false;
      restart.disabled = false;
    }
  };
  restart.addEventListener('click', () => { restart.disabled = true; void begin(); });
  // The form only ever posts to IdentityApi, which validates the pending request itself.
  form.action = accountLoginUrl();

  const authRequest = parameters.get('authRequest');
  // A credential change (account page) ended the session; say why a sign-in is needed again.
  const notice = sessionStorage.getItem(signInNoticeStorageKey);
  if (authRequest && notice) {
    sessionStorage.removeItem(signInNoticeStorageKey);
    status.textContent = notice;
  }
  if (authRequest) {
    form.querySelector<HTMLInputElement>('[name="authRequest"]')!.value = authRequest;
    form.hidden = false;
    const code = parameters.get('error');
    const retryAfter = Number(parameters.get('retryAfter'));
    if (code === 'locked') {
      showError(`Too many failed sign-in attempts for this login. Try again in ${formatSeconds(Number.isFinite(retryAfter) && retryAfter > 0 ? retryAfter : 60)}.`);
    } else if (code === 'invalid') {
      showError('Invalid login or password.');
    }
    // The browser submits the form itself (a first-party post to the identity server); only
    // guard against double submission.
    form.addEventListener('submit', event => {
      if (button.disabled || !form.reportValidity()) { event.preventDefault(); return; }
      button.disabled = true; button.textContent = 'Signing in…'; error.hidden = true;
    });
    // Back/forward cache can restore the page mid-submission.
    window.addEventListener('pageshow', event => { if (event.persisted) { button.disabled = false; button.textContent = 'Enter dashboard'; } });
    form.querySelector<HTMLInputElement>('[name="login"]')!.focus();
    return;
  }

  const errorId = parameters.get('errorId');
  if (errorId) {
    showError(await describeSignInError(errorId));
    restart.hidden = false;
    return;
  }
  if (parameters.get('error') === 'request') {
    showError('The sign-in request expired. Start again.');
    restart.hidden = false;
    return;
  }
  if (parameters.has('signedOut')) {
    status.textContent = 'You signed out.';
    restart.hidden = false;
    return;
  }
  await begin();
};
