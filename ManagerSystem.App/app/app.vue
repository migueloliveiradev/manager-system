<script setup lang="ts">
const links = [
  { label: 'Dashboard', to: '/dashboard', icon: 'i-lucide-layout-dashboard' },
  { label: 'Meus Projetos', to: '/projects', icon: 'i-lucide-briefcase' },
  { label: 'Minhas Tarefas', to: '/tasks', icon: 'i-lucide-list-todo' },
  { label: 'Calendário', to: '/calendar', icon: 'i-lucide-calendar' },
  { label: 'Configurações', to: '/settings', icon: 'i-lucide-settings' }
]
</script>

<template>
  <UApp>
    <div class="min-h-screen flex bg-default">
      <aside class="w-64 border-r border-default p-4 hidden md:block">
        <h1 class="font-semibold text-lg mb-4">
          Manager System
        </h1>
        <UNavigationMenu
          orientation="vertical"
          :items="links"
        />
      </aside>
      <main class="flex-1 p-4 md:p-6">
        <NuxtPage />
      </main>
    </div>
  </UApp>
</template>
