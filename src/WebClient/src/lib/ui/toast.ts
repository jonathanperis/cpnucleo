import { createIcon, type IconName } from '~/lib/icons';

export interface ToastAction {
  label: string;
  /** Runs once; the toast closes first. */
  run: () => void | Promise<void>;
}

export interface ToastOptions {
  tone?: 'success' | 'info' | 'danger';
  action?: ToastAction;
  /** Link shown next to the message (for example "Open" for a record just created). */
  link?: { label: string; href: string };
  durationMs?: number;
}

const toneIcons: Record<NonNullable<ToastOptions['tone']>, { icon: IconName; className: string }> = {
  success: { icon: 'check', className: 'text-success' },
  info: { icon: 'alert', className: 'text-accent-text' },
  danger: { icon: 'alert', className: 'text-danger' },
};

const DEFAULT_DURATION_MS = 6000;
const ACTION_DURATION_MS = 10000;

/**
 * Short confirmations in the page's polite live region (AppLayout's `[data-toasts]`). Messages are
 * set with textContent. Timers pause while the pointer or keyboard focus is inside a toast, so an
 * Undo button never disappears under the cursor.
 */
export const showToast = (message: string, { tone = 'success', action, link, durationMs }: ToastOptions = {}): (() => void) => {
  const region = document.querySelector<HTMLElement>('[data-toasts]');
  if (!region) return () => undefined;

  const toast = document.createElement('div');
  toast.className = 'toast reveal';
  toast.dataset.toast = tone;
  const { icon, className } = toneIcons[tone];
  const text = document.createElement('p');
  text.className = 'min-w-0 flex-1 text-sm';
  text.textContent = message;
  toast.append(createIcon(icon, `size-4 flex-none ${className}`), text);

  let timer: ReturnType<typeof setTimeout> | undefined;
  const close = () => {
    clearTimeout(timer);
    toast.remove();
  };

  if (link) {
    const anchor = document.createElement('a');
    anchor.href = link.href;
    anchor.className = 'btn btn-ghost btn-sm';
    anchor.textContent = link.label;
    toast.append(anchor);
  }
  if (action) {
    const button = document.createElement('button');
    button.type = 'button';
    button.className = 'btn btn-secondary btn-sm';
    button.dataset.toastAction = '';
    button.textContent = action.label;
    button.addEventListener('click', () => { close(); void action.run(); }, { once: true });
    toast.append(button);
  }
  const dismiss = document.createElement('button');
  dismiss.type = 'button';
  dismiss.className = 'btn btn-ghost btn-icon';
  dismiss.setAttribute('aria-label', 'Dismiss notification');
  dismiss.append(createIcon('close'));
  dismiss.addEventListener('click', close, { once: true });
  toast.append(dismiss);

  const duration = durationMs ?? (action ? ACTION_DURATION_MS : DEFAULT_DURATION_MS);
  const start = () => { clearTimeout(timer); timer = setTimeout(close, duration); };
  const pause = () => clearTimeout(timer);
  toast.addEventListener('pointerenter', pause);
  toast.addEventListener('pointerleave', start);
  toast.addEventListener('focusin', pause);
  toast.addEventListener('focusout', event => { if (!toast.contains(event.relatedTarget as Node | null)) start(); });

  region.append(toast);
  start();
  return close;
};
