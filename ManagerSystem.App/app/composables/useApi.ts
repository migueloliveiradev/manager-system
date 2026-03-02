export const useApi = () => {
  const config = useRuntimeConfig()
  const { token } = useAuth()

  return async <T>(url: string, request: {
    method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
    body?: unknown
    headers?: HeadersInit
  } = {}) => {
    const headers = new Headers(request.headers)
    if (token.value) headers.set('Authorization', `Bearer ${token.value}`)
    return await $fetch<T>(`${config.public.apiBase}${url}`, { ...request, headers })
  }
}
