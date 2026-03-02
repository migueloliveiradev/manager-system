<script setup lang="ts">
import type { Project, User, TaskColumn, WorkTask } from '~/types/api'

const props = defineProps<{
  open: boolean
  task?: WorkTask | null
  projects: Project[]
  users: User[]
  columns: TaskColumn[]
  defaultProjectId?: string
}>()

const emit = defineEmits<{
  close: []
  save: [payload: {
    title: string
    description: string
    priority: number
    dueDate: string | null
    projectId: string
    assigneeId: string | null
    statusId: string
    progress: number
  }]
}>()

const form = reactive({
  title: '',
  description: '',
  priority: 2,
  dueDate: '',
  projectId: '',
  assigneeId: '',
  statusId: '',
  progress: 0
})

watch(() => [props.task, props.defaultProjectId, props.columns] as const, () => {
  form.title = props.task?.title || ''
  form.description = props.task?.description || ''
  form.priority = props.task?.priority || 2
  form.dueDate = props.task?.dueDate?.slice(0, 10) || ''
  form.projectId = props.task?.projectId || props.defaultProjectId || props.projects[0]?.id || ''
  form.assigneeId = props.task?.assigneeId || ''
  form.statusId = props.task?.statusId || props.columns[0]?.id || ''
  form.progress = props.task?.progress || 0
}, { immediate: true })
</script>

<template>
  <UModal
    :open="open"
    :title="task ? 'Editar tarefa' : 'Nova tarefa'"
    @update:open="v => !v && emit('close')"
  >
    <template #body>
      <div class="space-y-3">
        <UInput
          v-model="form.title"
          placeholder="Título"
        />
        <UTextarea
          v-model="form.description"
          placeholder="Descrição"
        />
        <USelect
          v-model="form.projectId"
          :items="projects.map(p => ({ label: p.name, value: p.id }))"
        />
        <USelect
          v-model="form.assigneeId"
          :items="[{ label: 'Sem responsável', value: '' }, ...users.map(u => ({ label: u.fullName, value: u.id }))]"
        />
        <USelect
          v-model="form.priority"
          :items="[{ label: 'Baixa', value: 1 }, { label: 'Média', value: 2 }, { label: 'Alta', value: 3 }, { label: 'Urgente', value: 4 }]"
        />
        <USelect
          v-model="form.statusId"
          :items="columns.map(c => ({ label: c.name, value: c.id }))"
        />
        <UInput
          v-model="form.dueDate"
          type="date"
        />
        <UInput
          v-model="form.progress"
          type="number"
          min="0"
          max="100"
        />
      </div>
    </template>
    <template #footer>
      <UButton @click="emit('save', { title: form.title, description: form.description, priority: form.priority, dueDate: form.dueDate || null, projectId: form.projectId, assigneeId: form.assigneeId || null, statusId: form.statusId, progress: Number(form.progress || 0) })">
        Salvar
      </UButton>
    </template>
  </UModal>
</template>
