<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { registerAccount } from '../api/account'

const router = useRouter()
const username = ref('')
const password = ref('')
const nickname = ref('')
const sex = ref(false)
const loading = ref(false)
const error = ref<string | null>(null)

async function submit() {
  loading.value = true
  error.value = null
  try {
    await registerAccount({ username: username.value, password: password.value, nickname: nickname.value, sex: sex.value })
    await router.push('/login')
  } catch (cause) {
    error.value = cause instanceof Error ? cause.message : 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <main class="page">
    <section class="card">
      <h1>Create account</h1>
      <form @submit.prevent="submit">
        <label>Username<input v-model="username" required minlength="3" maxlength="64" pattern="[A-Za-z0-9_]+" /></label>
        <label>Password<input v-model="password" type="password" required minlength="6" maxlength="256" /></label>
        <label>Nickname<input v-model="nickname" required maxlength="32" /></label>
        <label><input v-model="sex" type="checkbox" /> Female character</label>
        <p v-if="error" class="error">{{ error }}</p>
        <button :disabled="loading" type="submit">{{ loading ? 'Creating…' : 'Create account' }}</button>
      </form>
      <button type="button" @click="router.push('/login')">Back to login</button>
    </section>
  </main>
</template>

<style scoped>
.page { min-height: 100vh; display: grid; place-items: center; padding: 24px; }
.card { width: min(420px, 100%); padding: 32px; border: 1px solid #ddd; border-radius: 16px; background: #fff; }
form { display: grid; gap: 16px; margin: 24px 0; }
label { display: grid; gap: 8px; font-weight: 600; }
input { padding: 12px; border: 1px solid #ccc; border-radius: 10px; }
button { padding: 12px 16px; border: 0; border-radius: 10px; cursor: pointer; }
.error { color: #b42318; }
</style>
