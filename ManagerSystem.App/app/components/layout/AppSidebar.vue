<script setup lang="ts">
const links = [
  { label: 'Dashboard', to: '/dashboard', icon: 'i-lucide-layout-dashboard' },
  { label: 'Meus Projetos', to: '/projects', icon: 'i-lucide-briefcase' },
  { label: 'Minhas Tarefas', to: '/tasks', icon: 'i-lucide-list-todo' },
  { label: 'Calendário', to: '/calendar', icon: 'i-lucide-calendar' },
  { label: 'Configurações', to: '/settings', icon: 'i-lucide-settings' }
]

const { logout } = useAuth()
const onLogout = async () => {
  logout()
  await navigateTo('/auth')
}
</script>

<template>
  <aside class="w-64 border-r border-default p-4 hidden md:block">
    <h1 class="font-semibold text-lg mb-4">
      Manager System
    </h1>
    <UNavigationMenu
      orientation="vertical"
      :items="links"
    />
    <UButton
      class="mt-4"
      color="neutral"
      variant="outline"
      block
      @click="onLogout"
    >
      Sair
    </UButton>
  </aside>
</template>
