<template>
  <div class="space-y-4">
    <h2 class="text-2xl font-semibold">
      Dashboard Geral
    </h2>
    <div class="grid gap-4 md:grid-cols-3">
      <UCard>
        <template #header>
          Tarefas Pendentes
        </template><p class="text-3xl font-bold">
          24
        </p>
      </UCard>
      <UCard>
        <template #header>
          Concluídas
        </template><p class="text-3xl font-bold text-success">
          48
        </p>
      </UCard>
      <UCard>
        <template #header>
          Atrasadas
        </template><p class="text-3xl font-bold text-error">
          7
        </p>
      </UCard>
    </div>
    <UCard>
      <template #header>
        Projetos com alta urgência
      </template>
      <ul class="space-y-2">
        <li>Portal Comercial - 6 tarefas urgentes</li>
        <li>App Mobile - 4 tarefas urgentes</li>
      </ul>
    </UCard>
  </div>
</template>
