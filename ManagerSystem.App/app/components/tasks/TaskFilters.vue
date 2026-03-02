<script setup lang="ts">
import type { Project, User, TaskColumn } from '~/types/api'

defineProps<{
  projects: Project[]
  users: User[]
  columns: TaskColumn[]
  projectId: string
  assigneeId: string
  priority: number
  statusId: string
  dueDateUntil: string
}>()

const emit = defineEmits<{
  'update:projectId': [string]
  'update:assigneeId': [string]
  'update:priority': [number]
  'update:statusId': [string]
  'update:dueDateUntil': [string]
}>()
</script>

<template>
  <UCard>
    <div class="grid md:grid-cols-5 gap-3">
      <USelect
        :model-value="projectId"
        :items="[{ label: 'Todos projetos', value: '' }, ...projects.map(p => ({ label: p.name, value: p.id }))]"
        @update:model-value="emit('update:projectId', String($event || ''))"
      />
      <USelect
        :model-value="assigneeId"
        :items="[{ label: 'Todos responsáveis', value: '' }, ...users.map(u => ({ label: u.fullName, value: u.id }))]"
        @update:model-value="emit('update:assigneeId', String($event || ''))"
      />
      <USelect
        :model-value="priority"
        :items="[{ label: 'Todas prioridades', value: 0 }, { label: 'Baixa', value: 1 }, { label: 'Média', value: 2 }, { label: 'Alta', value: 3 }, { label: 'Urgente', value: 4 }]"
        @update:model-value="emit('update:priority', Number($event || 0))"
      />
      <USelect
        :model-value="statusId"
        :items="[{ label: 'Todos status', value: '' }, ...columns.map(c => ({ label: c.name, value: c.id }))]"
        @update:model-value="emit('update:statusId', String($event || ''))"
      />
      <UInput
        :model-value="dueDateUntil"
        type="date"
        @update:model-value="emit('update:dueDateUntil', String($event || ''))"
      />
    </div>
  </UCard>
</template>
