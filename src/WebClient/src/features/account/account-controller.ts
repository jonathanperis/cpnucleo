import { accountClient, type AccountProfile } from '~/lib/api/account-client';
import { ApiError, getSessionClaims, signOut, type SessionClaims } from '~/lib/api/http-client';
import { formatUtcDay } from '~/lib/dates';
import { showToast } from '~/lib/ui/toast';
import { signInNoticeStorageKey } from '~/features/auth/login-controller';

/** Notice code for the sign-in page after a credential change ended the session. */
export const credentialsChangedNotice = 'credentials-changed';

const fieldMessages = (error: unknown, field: string) =>
  error instanceof ApiError ? (Object.entries(error.fieldErrors).find(([key]) => key.toLowerCase() === field.toLowerCase())?.[1] ?? []) : [];

/** Shows a form's field and summary errors next to the controls (same pattern as the CRUD forms). */
const reportErrors = (form: HTMLFormElement, error: unknown, fields: string[]) => {
  const summary = form.querySelector<HTMLElement>('[data-form-error]')!;
  let first: HTMLElement | undefined;
  for (const name of fields) {
    const control = form.elements.namedItem(name) as HTMLInputElement;
    const message = form.querySelector<HTMLElement>(`[data-field-error="${name}"]`)!;
    const messages = fieldMessages(error, name);
    message.textContent = messages.join(' ');
    message.hidden = messages.length === 0;
    if (messages.length > 0) { control.setAttribute('aria-invalid', 'true'); first ??= control; } else control.removeAttribute('aria-invalid');
  }
  summary.textContent = error instanceof Error ? error.message : 'Unable to save the changes.';
  summary.hidden = false;
  first?.focus();
};

const clearErrors = (form: HTMLFormElement) => {
  form.querySelector<HTMLElement>('[data-form-error]')!.hidden = true;
  for (const message of form.querySelectorAll<HTMLElement>('[data-field-error]')) { message.hidden = true; message.textContent = ''; }
  for (const control of form.querySelectorAll('[aria-invalid]')) control.removeAttribute('aria-invalid');
};

/** "Your account": profile facts, display name and password, for the signed-in person only. */
export const mountAccountPage = (root: HTMLElement, session: SessionClaims | null = getSessionClaims(), onPasswordChanged: () => void = () => void signOut()): (() => void) => {
  const query = <T extends Element>(selector: string) => root.querySelector<T>(selector)!;
  const nameForm = query<HTMLFormElement>('[data-name-form]');
  const passwordForm = query<HTMLFormElement>('[data-password-form]');
  const lifetime = new AbortController();
  let profile: AccountProfile | null = null;

  const showProfile = (value: AccountProfile) => {
    profile = value;
    query<HTMLElement>('[data-profile-name]').textContent = value.name;
    query<HTMLElement>('[data-profile-login]').textContent = value.login;
    query<HTMLElement>('[data-profile-role]').textContent = session?.isAdmin ? 'Administrator' : 'Member';
    query<HTMLElement>('[data-profile-since]').textContent = formatUtcDay(value.createdAt);
    query<HTMLElement>('[data-profile-initial]').textContent = value.name.trim().charAt(0) || value.login.charAt(0) || '?';
    (nameForm.elements.namedItem('name') as HTMLInputElement).value = value.name;
  };

  void accountClient.getProfile(lifetime.signal).then(showProfile).catch(error => {
    if (lifetime.signal.aborted) return;
    const alert = query<HTMLElement>('[data-account-error]');
    alert.textContent = error instanceof Error ? error.message : 'Unable to load your account.';
    alert.hidden = false;
  });

  nameForm.addEventListener('submit', async event => {
    event.preventDefault();
    clearErrors(nameForm);
    if (!nameForm.reportValidity()) return;
    const button = nameForm.querySelector<HTMLButtonElement>('[type="submit"]')!;
    const name = (nameForm.elements.namedItem('name') as HTMLInputElement).value.trim();
    button.disabled = true;
    try {
      await accountClient.updateName(name);
      if (lifetime.signal.aborted) return;
      if (profile) showProfile({ ...profile, name });
      showToast('Your name was updated.');
    } catch (error) {
      if (!lifetime.signal.aborted) reportErrors(nameForm, error, ['name']);
    } finally { button.disabled = false; }
  }, { signal: lifetime.signal });

  const newPassword = passwordForm.elements.namedItem('newPassword') as HTMLInputElement;
  const confirmPassword = passwordForm.elements.namedItem('confirmPassword') as HTMLInputElement;
  const checkMatch = () => confirmPassword.setCustomValidity(confirmPassword.value && confirmPassword.value !== newPassword.value ? 'The passwords do not match.' : '');
  newPassword.addEventListener('input', checkMatch, { signal: lifetime.signal });
  confirmPassword.addEventListener('input', checkMatch, { signal: lifetime.signal });

  passwordForm.addEventListener('submit', async event => {
    event.preventDefault();
    clearErrors(passwordForm);
    checkMatch();
    if (!passwordForm.reportValidity()) return;
    const button = passwordForm.querySelector<HTMLButtonElement>('[type="submit"]')!;
    const current = (passwordForm.elements.namedItem('currentPassword') as HTMLInputElement).value;
    button.disabled = true; button.textContent = 'Changing…';
    try {
      await accountClient.changePassword(current, newPassword.value);
      if (lifetime.signal.aborted) return;
      passwordForm.reset();
      try { sessionStorage.setItem(signInNoticeStorageKey, credentialsChangedNotice); } catch { /* storage unavailable */ }
      // The new password changed the security stamp: every token of the account is now invalid.
      onPasswordChanged();
    } catch (error) {
      if (!lifetime.signal.aborted) reportErrors(passwordForm, error, ['currentPassword', 'newPassword']);
    } finally { button.disabled = false; button.textContent = 'Change password'; }
  }, { signal: lifetime.signal });

  return () => lifetime.abort();
};
