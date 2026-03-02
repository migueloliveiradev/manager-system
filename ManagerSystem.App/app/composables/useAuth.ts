import type { AuthResponse, BaseResponse } from '~/types/api'

export const useAuth = () => {
  const config = useRuntimeConfig()
  const token = useCookie<string | null>('ms_access_token', { default: () => null })
  const refreshToken = useCookie<string | null>('ms_refresh_token', { default: () => null })

  const login = async (email: string, password: string) => {
    const response = await $fetch<BaseResponse<AuthResponse>>(`${config.public.apiBase}/api/auth/login`, { method: 'POST', body: { email, password } })
    if (response.hasErrors || !response.data) throw new Error(response.errors.join(', '))
    token.value = response.data.accessToken
    refreshToken.value = response.data.refreshToken
  }

  const register = async (fullName: string, email: string, password: string, role = 'Manager') => {
    const response = await $fetch<BaseResponse<AuthResponse>>(`${config.public.apiBase}/api/auth/register`, { method: 'POST', body: { fullName, email, password, role } })
    if (response.hasErrors || !response.data) throw new Error(response.errors.join(', '))
    token.value = response.data.accessToken
    refreshToken.value = response.data.refreshToken
  }

  const logout = () => {
    token.value = null
    refreshToken.value = null
  }

  return { token, refreshToken, login, register, logout }
}
