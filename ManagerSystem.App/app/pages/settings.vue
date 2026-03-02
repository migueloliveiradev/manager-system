<template>
  <div class="space-y-4 max-w-xl">
    <h2 class="text-2xl font-semibold">
      Configurações
    </h2>
    <UCard>
      <UFormField label="Nome">
        <UInput placeholder="Nome do usuário" />
      </UFormField>
      <UFormField
        class="mt-3"
        label="E-mail"
      >
        <UInput placeholder="email@empresa.com" />
      </UFormField>
      <UButton class="mt-4">
        Salvar
      </UButton>
    </UCard>
  </div>
</template>
