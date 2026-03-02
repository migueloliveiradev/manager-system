<script setup lang="ts">
const isRegister = ref(false)
const loading = ref(false)
const error = ref('')
const form = reactive({ fullName: '', email: '', password: '' })
const { login, register } = useAuth()

const submit = async () => {
  loading.value = true
  error.value = ''
  try {
    if (isRegister.value) await register(form.fullName, form.email, form.password)
    else await login(form.email, form.password)
    await navigateTo('/dashboard')
  } catch (err: unknown) {
    error.value = err instanceof Error ? err.message : 'Falha na autenticação.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="max-w-md mx-auto mt-16">
    <UCard>
      <template #header>
        <h2 class="text-xl font-semibold">
          {{ isRegister ? 'Criar conta' : 'Entrar' }}
        </h2>
      </template>
      <div class="space-y-3">
        <UInput
          v-if="isRegister"
          v-model="form.fullName"
          placeholder="Nome completo"
        />
        <UInput
          v-model="form.email"
          placeholder="email@empresa.com"
        />
        <UInput
          v-model="form.password"
          type="password"
          placeholder="Senha"
        />
        <UAlert
          v-if="error"
          color="error"
          :description="error"
        />
        <UButton
          block
          :loading="loading"
          @click="submit"
        >
          {{ isRegister ? 'Registrar e entrar' : 'Entrar' }}
        </UButton>
        <UButton
          block
          color="neutral"
          variant="ghost"
          @click="isRegister = !isRegister"
        >
          {{ isRegister ? 'Já tenho conta' : 'Criar nova conta' }}
        </UButton>
      </div>
    </UCard>
  </div>
</template>
