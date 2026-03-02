<script setup lang="ts">
import type { BaseResponse, WorkTask, Project } from '~/types/api'

const api = useApi()
const tasks = ref<WorkTask[]>([])
const projects = ref<Project[]>([])

const load = async () => {
  const limitDate = new Date()
  limitDate.setDate(limitDate.getDate() + 30)
  const [taskRes, projectRes] = await Promise.all([
    api<BaseResponse<WorkTask[]>>(`/api/tasks?dueDateUntil=${limitDate.toISOString()}`),
    api<BaseResponse<Project[]>>('/api/projects')
  ])
  tasks.value = (taskRes.data || []).filter(x => x.dueDate)
  projects.value = projectRes.data || []
}

await load()

const projectMap = computed(() => Object.fromEntries(projects.value.map(x => [x.id, x.name])))
const sortedTasks = computed(() => [...tasks.value].sort((a, b) => new Date(a.dueDate || '').getTime() - new Date(b.dueDate || '').getTime()))
</script>

<template>
  <div class="space-y-4">
    <h2 class="text-2xl font-semibold">
      Calendário
    </h2>
    <UCard>
      <ul class="space-y-2">
        <li
          v-for="task in sortedTasks"
          :key="task.id"
        >
          {{ new Date(task.dueDate || '').toLocaleDateString() }} - {{ task.title }} ({{ projectMap[task.projectId] || 'Projeto' }})
        </li>
        <li v-if="!sortedTasks.length">
          Nenhuma tarefa com prazo nos próximos 30 dias.
        </li>
      </ul>
    </UCard>
  </div>
</template>
