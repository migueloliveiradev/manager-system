<script setup lang="ts">
import type { BaseResponse, Project } from '~/types/api'

const api = useApi()
const projects = ref<Project[]>([])
const search = ref('')
const status = ref(0)
const loading = ref(false)
const modalOpen = ref(false)
const editing = ref<Project | null>(null)

const loadProjects = async () => {
  loading.value = true
  const query = new URLSearchParams()
  if (search.value) query.set('search', search.value)
  if (status.value) query.set('status', String(status.value))
  const response = await api<BaseResponse<Project[]>>(`/api/projects?${query.toString()}`)
  projects.value = response.data || []
  loading.value = false
}

const saveProject = async (payload: { name: string, description: string, status: number }) => {
  if (editing.value) {
    await api(`/api/projects/${editing.value.id}`, { method: 'PUT', body: payload })
  } else {
    await api('/api/projects', { method: 'POST', body: payload })
  }
  modalOpen.value = false
  editing.value = null
  await loadProjects()
}

const removeProject = async (id: string) => {
  await api(`/api/projects/${id}`, { method: 'DELETE' })
  await loadProjects()
}

await loadProjects()
</script>

<template>
  <div class="space-y-4">
    <div class="flex items-center justify-between gap-3">
      <h2 class="text-2xl font-semibold">
        Meus Projetos
      </h2>
      <UButton @click="editing = null; modalOpen = true">
        Novo Projeto
      </UButton>
    </div>

    <UCard>
      <div class="grid gap-3 md:grid-cols-3">
        <UInput
          v-model="search"
          placeholder="Buscar projeto"
          @update:model-value="loadProjects"
        />
        <USelect
          v-model="status"
          :items="[{ label: 'Todos', value: 0 }, { label: 'Ativo', value: 1 }, { label: 'Arquivado', value: 2 }, { label: 'Concluído', value: 3 }]"
          @update:model-value="loadProjects"
        />
      </div>
    </UCard>

    <UProgress
      v-if="loading"
      animation="carousel"
    />

    <div class="grid gap-4 md:grid-cols-2">
      <UCard
        v-for="project in projects"
        :key="project.id"
      >
        <template #header>
          {{ project.name }}
        </template>
        <p>{{ project.description }}</p>
        <p class="text-sm text-muted mt-2">
          Status: {{ ['-', 'Ativo', 'Arquivado', 'Concluído'][project.status] }}
        </p>
        <div class="mt-3 flex gap-2">
          <UButton
            size="sm"
            variant="subtle"
            @click="editing = project; modalOpen = true"
          >
            Editar
          </UButton>
          <UButton
            size="sm"
            color="error"
            variant="soft"
            @click="removeProject(project.id)"
          >
            Excluir
          </UButton>
          <UButton
            size="sm"
            color="neutral"
            variant="outline"
            :to="`/tasks?projectId=${project.id}`"
          >
            Abrir Kanban
          </UButton>
        </div>
      </UCard>
    </div>

    <ProjectFormModal
      :open="modalOpen"
      :project="editing"
      @close="modalOpen = false"
      @save="saveProject"
    />
  </div>
</template>
