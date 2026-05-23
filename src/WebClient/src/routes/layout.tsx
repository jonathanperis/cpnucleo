import { component$, Slot } from '@builder.io/qwik';
import { AppShell } from '~/components/app-shell';

export default component$(() => (
  <AppShell>
    <Slot />
  </AppShell>
));
