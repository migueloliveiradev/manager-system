<script setup lang="ts">
import type { BaseResponse, User } from '~/types/api'

const api = useApi()
const userId = useCurrentUserId()
const profile = reactive({ fullName: '', profilePhotoUrl: '', isActive: true })
const password = reactive({ currentPassword: '', newPassword: '' })
const message = ref('')

const loadProfile = async () => {
  if (!userId.value) return
  const response = await api<BaseResponse<User>>(`/api/users/${userId.value}`)
  if (!response.data) return
  profile.fullName = response.data.fullName
  profile.profilePhotoUrl = response.data.profilePhotoUrl || ''
  profile.isActive = response.data.isActive
}

const saveProfile = async () => {
  if (!userId.value) return
  await api(`/api/users/${userId.value}`, { method: 'PUT', body: profile })
  message.value = 'Perfil atualizado com sucesso.'
}

const changePassword = async () => {
  if (!userId.value || !password.currentPassword || !password.newPassword) return
  await api(`/api/users/${userId.value}/password`, { method: 'PUT', body: password })
  password.currentPassword = ''
  password.newPassword = ''
  message.value = 'Senha alterada com sucesso.'
}

await loadProfile()
</script>

<template>
  <div class="space-y-4 max-w-xl">
    <h2 class="text-2xl font-semibold">
      Configurações
    </h2>
    <UCard>
      <div class="space-y-3">
        <UInput
          v-model="profile.fullName"
          placeholder="Nome"
        />
        <UInput
          v-model="profile.profilePhotoUrl"
          placeholder="URL da foto"
        />
        <UButton @click="saveProfile">
          Salvar perfil
        </UButton>
      </div>
    </UCard>

    <UCard>
      <template #header>
        Alterar senha
      </template>
      <div class="space-y-3">
        <UInput
          v-model="password.currentPassword"
          type="password"
          placeholder="Senha atual"
        />
        <UInput
          v-model="password.newPassword"
          type="password"
          placeholder="Nova senha"
        />
        <UButton @click="changePassword">
          Atualizar senha
        </UButton>
      </div>
    </UCard>

    <UAlert
      v-if="message"
      color="success"
      :description="message"
    />
  </div>
</template>
