<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const auth = useAuthStore()
const username = ref('')
const password = ref('')

async function submit() {
  const ok = await auth.login(username.value, password.value)
  if (ok) await router.push('/')
}
</script>

<template>
  <main class="auth-page">
    <section class="auth-card">
      <h1>登入</h1>
      <p class="muted">DDTank 帳號登入</p>

      <form @submit.prevent="submit">
        <label>
          帳號
          <input v-model="username" autocomplete="username" required maxlength="64" />
        </label>

        <label>
          密碼
          <input v-model="password" type="password" autocomplete="current-password" required maxlength="256" />
        </label>

        <p v-if="auth.error" class="error">{{ auth.error }}</p>
        <button :disabled="auth.loading" type="submit">
          {{ auth.loading ? '登入中…' : '登入' }}
        </button>
      </form>

      <button class="link-button" type="button" @click="router.push('/register')">
        建立新帳號
      </button>
    </section>
  </main>
</template>

<style scoped>
.auth-page { min-height: 100vh; display: grid; place-items: center; padding: 24px; }
.auth-card { width: min(420px, 100%); padding: 32px; border: 1px solid #ddd; border-radius: 16px; background: #fff; box-shadow: 0 12px 40px rgba(0,0,0,.08); }
form { display: grid; gap: 16px; margin-top: 24px; }
label { display: grid; gap: 8px; font-weight: 600; }
input { padding: 12px 14px; border: 1px solid #ccc; border-radius: 10px; font: inherit; }
button { padding: 12px 16px; border: 0; border-radius: 10px; font: inherit; cursor: pointer; }
.error { color: #b42318; margin: 0; }
.muted { color: #667085; }
.link-button { margin-top: 12px; width: 100%; background: transparent; }
</style>
