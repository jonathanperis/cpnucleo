import { component$ } from '@builder.io/qwik';
import { CrudPage } from '~/features/crud/crud-page';
import { findResource } from '~/lib/api/resource-metadata';

export default component$(() => <CrudPage resource={findResource('userAssignments')} />);
