import { describe, expect, it } from 'vitest';
import { getCrudFormElement } from './crud-form';

const formElement = { tagName: 'FORM' } as HTMLFormElement;
const documentLikeTarget = { tagName: 'DOCUMENT' } as unknown as EventTarget;
const buttonLikeTarget = { tagName: 'BUTTON' } as unknown as EventTarget;

describe('CRUD form helpers', () => {
  it('uses Qwik delegated currentTarget argument instead of the document-level event currentTarget', () => {
    const delegatedSubmitEvent = {
      currentTarget: documentLikeTarget,
      target: buttonLikeTarget,
    } as SubmitEvent;

    expect(getCrudFormElement(delegatedSubmitEvent, formElement)).toBe(formElement);
  });
});
