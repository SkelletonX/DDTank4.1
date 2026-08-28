import { apiFetch } from './client'

export interface RegisterPayload {
  username: string
  password: string
  nickname: string
  sex: boolean
}

export async function registerAccount(payload: RegisterPayload) {
  return apiFetch<{ ok: true }>('/api/account/register', {
    method: 'POST',
    body: JSON.stringify(payload),
  })
}
