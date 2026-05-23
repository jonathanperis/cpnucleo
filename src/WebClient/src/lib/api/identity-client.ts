import { IDENTITY_API_BASE_URL } from '../config';
import { requestJson, setStoredToken } from './http-client';

export interface LoginResponse {
  token: string;
}

export const login = async (loginName: string, password: string, baseUrl = IDENTITY_API_BASE_URL): Promise<LoginResponse> => {
  const response = await requestJson<LoginResponse>(`${baseUrl.replace(/\/$/, '')}/login`, {
    method: 'POST',
    token: null,
    body: JSON.stringify({ login: loginName, password }),
  });
  setStoredToken(response.token);
  return response;
};
