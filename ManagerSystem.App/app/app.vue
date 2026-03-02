<script setup lang="ts">
const route = useRoute()
</script>

<template>
  <UApp>
    <div class="min-h-screen flex bg-default">
      <AppSidebar v-if="route.path !== '/auth'" />
      <main class="flex-1 p-4 md:p-6">
        <NuxtPage />
      </main>
    </div>
  </UApp>
</template>
