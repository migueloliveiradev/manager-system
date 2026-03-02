export default defineNuxtRouteMiddleware((to) => {
  if (to.path === '/auth') return
  const token = useCookie<string | null>('ms_access_token')
  if (!token.value) return navigateTo('/auth')
})
