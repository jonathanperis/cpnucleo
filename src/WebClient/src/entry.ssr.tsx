import { renderToStream } from '@builder.io/qwik/server';
import { manifest } from '@qwik-client-manifest';
import Root from './root';

export default function (opts: Parameters<typeof renderToStream>[1]) {
  return renderToStream(<Root />, {
    manifest,
    ...opts,
  });
}
