<script setup lang="ts">
import type { BaseResponse, Project, WorkTask } from '~/types/api'

const api = useApi()
const loading = ref(true)
const projects = ref<Project[]>([])
const tasks = ref<WorkTask[]>([])

const load = async () => {
  loading.value = true
  const [projectsRes, tasksRes] = await Promise.all([
    api<BaseResponse<Project[]>>('/api/projects'),
    api<BaseResponse<WorkTask[]>>('/api/tasks')
  ])
  projects.value = projectsRes.data || []
  tasks.value = tasksRes.data || []
  loading.value = false
}

await load()

const pending = computed(() => tasks.value.filter(x => x.progress < 100).length)
const completed = computed(() => tasks.value.filter(x => x.progress === 100).length)
const delayed = computed(() => tasks.value.filter(x => x.progress < 100 && x.dueDate && new Date(x.dueDate) < new Date()).length)

const urgentProjects = computed(() => projects.value
  .map(project => ({
    project,
    urgentCount: tasks.value.filter(task => task.projectId === project.id && task.priority >= 3 && task.progress < 100).length
  }))
  .filter(x => x.urgentCount > 0)
  .sort((a, b) => b.urgentCount - a.urgentCount)
  .slice(0, 5))
</script>

<template>
  <div class="space-y-4">
    <h2 class="text-2xl font-semibold">
      Dashboard Geral
    </h2>

    <UProgress
      v-if="loading"
      animation="carousel"
    />

    <DashboardStats
      v-else
      :pending="pending"
      :completed="completed"
      :delayed="delayed"
    />

    <UCard>
      <template #header>
        Projetos com alta urgência
      </template>
      <ul class="space-y-2">
        <li
          v-for="item in urgentProjects"
          :key="item.project.id"
        >
          {{ item.project.name }} - {{ item.urgentCount }} tarefas críticas abertas
        </li>
        <li v-if="!urgentProjects.length">
          Nenhum projeto com urgência alta no momento.
        </li>
      </ul>
    </UCard>
  </div>
</template>
