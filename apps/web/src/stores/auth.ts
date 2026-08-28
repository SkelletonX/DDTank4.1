import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { apiFetch } from '../api/client'

export interface AuthUser {
  userId: number
  username: string
  nickname: string
  email: string | null
}

interface LoginResponse {
  user: AuthUser
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<AuthUser | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)
  const isLoggedIn = computed(() => user.value !== null)

  async function login(username: string, password: string) {
    loading.value = true
    error.value = null
    try {
      const result = await apiFetch<LoginResponse>('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify({ username, password }),
      })
      user.value = result.user
      return true
    } catch (cause) {
      error.value = cause instanceof Error ? cause.message : '登入失敗'
      return false
    } finally {
      loading.value = false
    }
  }

  async function restore() {
    try {
      const result = await apiFetch<{ user: { userId: number; username: string } }>('/api/auth/me')
      user.value = { ...result.user, nickname: result.user.username, email: null }
    } catch {
      user.value = null
    }
  }

  async function logout() {
    await apiFetch<{ ok: true }>('/api/auth/logout', { method: 'POST' })
    user.value = null
  }

  return { user, loading, error, isLoggedIn, login, restore, logout }
})
