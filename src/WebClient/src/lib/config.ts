export const WEBAPI_BASE_URL = import.meta.env.PUBLIC_WEBAPI_BASE_URL || 'https://api-cpnucleo.jonathanperis.tech/api';
export const IDENTITY_API_BASE_URL = import.meta.env.PUBLIC_IDENTITY_API_BASE_URL || 'https://identity-cpnucleo.jonathanperis.tech/api';
export const IDENTITY_API_ISSUER = import.meta.env.PUBLIC_IDENTITY_API_ISSUER || 'https://identity-cpnucleo.jonathanperis.tech';

export const withoutApiSuffix = (baseUrl: string) => baseUrl.replace(/\/api\/?$/, '');
