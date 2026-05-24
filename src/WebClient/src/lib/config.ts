export const WEBAPI_BASE_URL = import.meta.env.PUBLIC_WEBAPI_BASE_URL || 'http://localhost:9999/api';
export const IDENTITY_API_BASE_URL = import.meta.env.PUBLIC_IDENTITY_API_BASE_URL || 'http://localhost:5200/api';
export const IDENTITY_API_ISSUER = import.meta.env.PUBLIC_IDENTITY_API_ISSUER || 'https://identity-cpnucleo.jonathanperis.tech';

export const withoutApiSuffix = (baseUrl: string) => baseUrl.replace(/\/api\/?$/, '');
