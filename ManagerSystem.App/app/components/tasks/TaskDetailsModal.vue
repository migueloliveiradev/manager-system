<script setup lang="ts">
import type { WorkTask, TaskHistory, Comment } from '~/types/api'

defineProps<{
  open: boolean
  task: WorkTask | null
  history: TaskHistory[]
  comments: Comment[]
  commentText: string
}>()

const emit = defineEmits<{
  close: []
  updateCommentText: [string]
  addComment: []
  removeComment: [string]
}>()
</script>

<template>
  <UModal
    :open="open"
    title="Detalhes da tarefa"
    @update:open="v => !v && emit('close')"
  >
    <template #body>
      <div
        v-if="task"
        class="space-y-4"
      >
        <p class="font-semibold">
          {{ task.title }}
        </p>
        <p>{{ task.description }}</p>
        <p class="text-sm text-muted">
          Progresso: {{ task.progress }}%
        </p>

        <USeparator label="Comentários" />
        <div class="space-y-2">
          <div
            v-for="comment in comments"
            :key="comment.id"
            class="border border-default rounded p-2"
          >
            <div class="flex items-center justify-between gap-2">
              <p class="text-sm">
                {{ comment.content }}
              </p>
              <UButton
                size="xs"
                color="error"
                variant="soft"
                @click="emit('removeComment', comment.id)"
              >
                Excluir
              </UButton>
            </div>
          </div>
          <UInput
            :model-value="commentText"
            placeholder="Novo comentário"
            @update:model-value="emit('updateCommentText', String($event || ''))"
          />
          <UButton
            size="sm"
            @click="emit('addComment')"
          >
            Adicionar comentário
          </UButton>
        </div>

        <USeparator label="Histórico" />
        <ul class="list-disc pl-5 text-sm space-y-1">
          <li
            v-for="line in history"
            :key="line.id"
          >
            {{ line.action }} - {{ new Date(line.createdAtUtc).toLocaleString() }}
          </li>
        </ul>
      </div>
    </template>
  </UModal>
</template>
