// @vitest-environment jsdom
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { accountClient } from '~/lib/api/account-client';
import { ApiError } from '~/lib/api/http-client';
import { signInNoticeStorageKey } from '~/features/auth/login-controller';
import { showBuiltPage } from '~/test/page-markup';
import { credentialsChangedNotice, mountAccountPage } from './account-controller';

let stop = () => {};
const root = () => document.querySelector<HTMLElement>('[data-account]')!;
const control = (name: string) => root().querySelector<HTMLInputElement>(`[name="${name}"]`)!;
const submit = (selector: string) => root().querySelector<HTMLFormElement>(selector)!.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));

beforeEach(() => {
  showBuiltPage('account');
  vi.spyOn(accountClient, 'getProfile').mockResolvedValue({ id: 'me', name: 'Ana Lima', login: 'ana@cpnucleo.test', createdAt: '2026-01-02T00:00:00Z' });
});
afterEach(() => { stop(); vi.restoreAllMocks(); sessionStorage.clear(); document.body.replaceChildren(); });

describe('account page', () => {
  it('shows the profile and saves a new display name', async () => {
    const update = vi.spyOn(accountClient, 'updateName').mockResolvedValue();
    stop = mountAccountPage(root(), { sub: 'me', login: 'ana@cpnucleo.test', isAdmin: false });
    await vi.waitFor(() => expect(root().querySelector('[data-profile-name]')?.textContent).toBe('Ana Lima'));
    expect(root().querySelector('[data-profile-role]')?.textContent).toBe('Member');
    expect(control('name').value).toBe('Ana Lima');
    control('name').value = '  Ana L.  ';
    submit('[data-name-form]');
    await vi.waitFor(() => expect(update).toHaveBeenCalledWith('Ana L.'));
    await vi.waitFor(() => expect(root().querySelector('[data-profile-name]')?.textContent).toBe('Ana L.'));
  });

  it('requires the repeated password to match before calling the API', () => {
    const change = vi.spyOn(accountClient, 'changePassword').mockResolvedValue();
    stop = mountAccountPage(root(), null);
    control('currentPassword').value = 'Old@12345';
    control('newPassword').value = 'New@123456';
    control('confirmPassword').value = 'Different@1';
    submit('[data-password-form]');
    expect(control('confirmPassword').validationMessage).toBe('The passwords do not match.');
    expect(change).not.toHaveBeenCalled();
  });

  it('marks a wrong current password next to the field', async () => {
    vi.spyOn(accountClient, 'changePassword').mockRejectedValue(new ApiError(400, 'The current password is incorrect.', undefined, { fieldErrors: { currentPassword: ['The current password is incorrect.'] } }));
    stop = mountAccountPage(root(), null);
    control('currentPassword').value = 'Wrong@12345';
    control('newPassword').value = 'New@123456';
    control('confirmPassword').value = 'New@123456';
    submit('[data-password-form]');
    await vi.waitFor(() => expect(control('currentPassword').getAttribute('aria-invalid')).toBe('true'));
    expect(root().querySelector('[data-field-error="currentPassword"]')?.textContent).toBe('The current password is incorrect.');
    expect(document.activeElement).toBe(control('currentPassword'));
  });

  it('signs out after a password change and leaves a notice for the sign-in page', async () => {
    const change = vi.spyOn(accountClient, 'changePassword').mockResolvedValue();
    const signedOut = vi.fn();
    stop = mountAccountPage(root(), null, signedOut);
    control('currentPassword').value = 'Old@12345';
    control('newPassword').value = 'New@123456';
    control('confirmPassword').value = 'New@123456';
    submit('[data-password-form]');
    await vi.waitFor(() => expect(signedOut).toHaveBeenCalled());
    expect(change).toHaveBeenCalledWith('Old@12345', 'New@123456');
    expect(sessionStorage.getItem(signInNoticeStorageKey)).toBe(credentialsChangedNotice);
  });
});
