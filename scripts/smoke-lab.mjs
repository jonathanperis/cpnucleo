import assert from 'node:assert/strict';
import { randomUUID } from 'node:crypto';

const api = new URL(process.env.LAB_API_URL ?? 'http://localhost:5100/api/');
const identity = new URL(process.env.LAB_IDENTITY_URL ?? 'http://localhost:5200/api/');
for (const url of [api, identity]) {
  assert(['localhost', '127.0.0.1', '[::1]'].includes(url.hostname), 'Lab smoke accepts loopback URLs only.');
}
const response = await fetch(new URL('login', identity), {
  method: 'POST', redirect: 'error', headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ login: 'demo@cpnucleo.local', password: process.env.LAB_PASSWORD ?? 'LocalLearning@123' }),
});
assert.equal(response.status, 200, 'Local login must succeed after explicit lab seeding.');
const { token } = await response.json();
assert.equal(typeof token, 'string');
const headers = { Authorization: `Bearer ${token}` };
const call = (path, method = 'GET', body) => fetch(new URL(path, api), {
  method, headers: body ? { ...headers, 'Content-Type': 'application/json' } : headers,
  redirect: 'error', ...(body ? { body: JSON.stringify(body) } : {}),
});
const organizationResponse = await call('organizations?pageSize=1');
assert.equal(organizationResponse.status, 200, 'Authenticated organization listing failed.');
const organizations = await organizationResponse.json();
const organizationId = organizations.result.data[0]?.id;
assert(organizationId, 'The tiny seed must contain an organization.');
let id;
try {
  const created = await call('project', 'POST', { name: `Smoke-${randomUUID()}`, organizationId });
  assert.equal(created.status, 200, 'Project creation failed.');
  id = (await created.json()).project.id;
  const original = (await (await call(`project?id=${id}`)).json()).project;
  const change = { id, organizationId, name: 'Smoke updated', expectedVersion: original.updatedAt ?? original.createdAt };
  assert.equal((await call('project', 'PATCH', change)).status, 200, 'Versioned edit failed.');
  assert.equal((await call('project', 'PATCH', change)).status, 409, 'Stale edit must conflict.');
  const refreshed = await fetch(new URL('refresh', identity), { method: 'POST', headers, redirect: 'error' });
  assert.equal(refreshed.status, 200, 'Active session refresh failed.');
  assert.equal((await call('users?pageSize=1')).status, 200, 'Local administrator must manage users.');
  console.log('Local HTTP smoke passed: login, admin access, create/read/update, conflict and refresh.');
} finally {
  if (id) {
    assert.equal((await call('project', 'DELETE', { ids: [id] })).status, 200, 'Smoke-owned project cleanup failed.');
    assert.equal((await call(`project?id=${id}`)).status, 404, 'Removed project remains visible.');
  }
}
