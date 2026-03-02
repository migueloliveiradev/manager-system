<script setup lang="ts">
import type { Project } from '~/types/api'

const props = defineProps<{ open: boolean, project?: Project | null }>()
const emit = defineEmits<{ close: [], save: [payload: { name: string, description: string, status: number }] }>()

const form = reactive({ name: '', description: '', status: 1 })

watch(() => props.project, (project) => {
  form.name = project?.name || ''
  form.description = project?.description || ''
  form.status = project?.status || 1
}, { immediate: true })
</script>

<template>
  <UModal
    :open="open"
    :title="project ? 'Editar Projeto' : 'Novo Projeto'"
    @update:open="v => !v && emit('close')"
  >
    <template #body>
      <div class="space-y-3">
        <UInput
          v-model="form.name"
          placeholder="Nome do projeto"
        />
        <UTextarea
          v-model="form.description"
          placeholder="Descrição"
        />
        <USelect
          v-model="form.status"
          :items="[{ label: 'Ativo', value: 1 }, { label: 'Arquivado', value: 2 }, { label: 'Concluído', value: 3 }]"
        />
      </div>
    </template>
    <template #footer>
      <UButton @click="emit('save', { name: form.name, description: form.description, status: form.status })">
        Salvar
      </UButton>
    </template>
  </UModal>
</template>
