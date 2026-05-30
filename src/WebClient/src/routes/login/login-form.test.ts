import { describe, expect, it } from 'vitest';
import { DEFAULT_LOGIN, getLoginFormElement } from './login-form';

const formElement = { tagName: 'FORM' } as HTMLFormElement;
const documentLikeTarget = { tagName: 'DOCUMENT' } as unknown as EventTarget;
const buttonLikeTarget = { tagName: 'BUTTON' } as unknown as EventTarget;

describe('login form helpers', () => {
  it('uses Qwik delegated currentTarget argument instead of the document-level event currentTarget', () => {
    const delegatedSubmitEvent = {
      currentTarget: documentLikeTarget,
      target: buttonLikeTarget,
    } as SubmitEvent;

    expect(getLoginFormElement(delegatedSubmitEvent, formElement)).toBe(formElement);
  });

  it('does not ship source-known demo credentials in the login form', () => {
    expect(DEFAULT_LOGIN).toBe('');
  });
});
