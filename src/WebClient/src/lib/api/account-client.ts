import { WEBAPI_BASE_URL } from '../config';
import { requestJson } from './http-client';

export interface AccountProfile {
  id: string;
  name: string;
  login: string;
  createdAt?: string;
  updatedAt?: string | null;
}

const unwrap = <T>(payload: unknown): T => {
  const record = payload as { result?: unknown } | undefined;
  return (record && typeof record === 'object' && record.result && typeof record.result === 'object' ? record.result : payload) as T;
};

/** Self-service account endpoints: any signed-in person manages their own name and password. */
export const createAccountClient = (baseUrl = WEBAPI_BASE_URL) => {
  const root = baseUrl.replace(/\/$/, '');
  return {
    async getProfile(signal?: AbortSignal) {
      return unwrap<AccountProfile>(await requestJson<unknown>(`${root}/me`, { signal }));
    },
    async updateName(name: string) {
      await requestJson<unknown>(`${root}/me`, { method: 'PATCH', body: JSON.stringify({ name }) });
    },
    /** Changing the password ends every session of the account, this one included. */
    async changePassword(currentPassword: string, newPassword: string) {
      await requestJson<unknown>(`${root}/me/password`, { method: 'POST', body: JSON.stringify({ currentPassword, newPassword }) });
    },
  };
};

export const accountClient = createAccountClient();
