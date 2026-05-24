import { describe, expect, it } from 'vitest';
import { DEMO_LOGIN, DEMO_PASSWORD, getLoginFormElement } from './login-form';

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

  it('keeps demonstration credentials available for the prefilled login form', () => {
    expect(DEMO_LOGIN).toBe('demo@cpnucleo.local');
    expect(DEMO_PASSWORD).toBe('CpnucleoDemo2026!');
  });
});
