export const useCurrentUserId = () => {
  const { token } = useAuth()
  return computed(() => {
    if (!token.value) return ''
    try {
      const payload = JSON.parse(atob(token.value.split('.')[1]))
      return String(payload.sub || payload.nameid || '')
    } catch {
      console.warn('Unable to parse access token. Please sign in again.')
      return ''
    }
  })
}
