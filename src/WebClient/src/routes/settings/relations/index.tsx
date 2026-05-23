import { component$ } from '@builder.io/qwik';
import { CrudPage } from '~/features/crud/crud-page';
import { findResource } from '~/lib/api/resource-metadata';

export default component$(() => (
  <div class="space-y-10">
    <CrudPage resource={findResource('userProjects')} />
    <CrudPage resource={findResource('userAssignments')} />
    <CrudPage resource={findResource('assignmentImpediments')} />
  </div>
));
