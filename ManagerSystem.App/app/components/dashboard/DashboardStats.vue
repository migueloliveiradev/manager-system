<script setup lang="ts">
defineProps<{
  pending: number
  completed: number
  delayed: number
}>()
</script>

<template>
  <div class="grid gap-4 md:grid-cols-3">
    <UCard>
      <template #header>
        Tarefas Pendentes
      </template>
      <p class="text-3xl font-bold">
        {{ pending }}
      </p>
    </UCard>
    <UCard>
      <template #header>
        Concluídas
      </template>
      <p class="text-3xl font-bold text-success">
        {{ completed }}
      </p>
    </UCard>
    <UCard>
      <template #header>
        Atrasadas
      </template>
      <p class="text-3xl font-bold text-error">
        {{ delayed }}
      </p>
    </UCard>
  </div>
</template>
