<script setup lang="ts">
import { ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useAuthStore } from '../stores/auth'
import SiteHeader from '../components/SiteHeader.vue'
import LegacyFlashLauncher from '../components/LegacyFlashLauncher.vue'
import LoginPanel from '../components/LoginPanel.vue'
import FacebookWidget from '../components/FacebookWidget.vue'
import HeroSlider from '../components/HeroSlider.vue'
import NewsPanel from '../components/NewsPanel.vue'
import ActivityPanel from '../components/ActivityPanel.vue'
import IntroBanner from '../components/IntroBanner.vue'
import WeaponShowcase from '../components/WeaponShowcase.vue'
import SiteFooter from '../components/SiteFooter.vue'
import AppModal from '../components/AppModal.vue'

const auth = useAuthStore()
const { loading: loginLoading, error: loginError } = storeToRefs(auth)
const registerOpen = ref(false)
const recoverOpen = ref(false)

async function handleLogin(username: string, password: string) {
  if (!username || !password) return
  await auth.login(username, password)
}
</script>

<template>
  <div class="wrap-box index-bg">
    <div class="w980 clearfix">
      <SiteHeader />

      <div class="w980 clearfix">
        <div class="index-left f-l">
          <div class="login p-r clearfix">
            <LegacyFlashLauncher />
            <LoginPanel
              :loading="loginLoading"
              :error="loginError"
              @login="handleLogin"
              @register="registerOpen = true"
              @recover="recoverOpen = true"
            />
            <AppModal :open="registerOpen" title="Criar uma Conta" @close="registerOpen = false">
              <div class="modal-body">
                <p>Registro de conta將在 API 化階段接入。</p>
              </div>
            </AppModal>
            <AppModal :open="recoverOpen" title="Recuperar senha" @close="recoverOpen = false">
              <div class="modal-body">
                <p>密碼恢復將在 API 化階段接入。</p>
              </div>
            </AppModal>
          </div>
          <FacebookWidget />
        </div>

        <div class="index-rgt f-r">
          <HeroSlider />
          <NewsPanel />
          <ActivityPanel />
          <IntroBanner />
          <WeaponShowcase />
        </div>
      </div>
    </div>
  </div>

  <SiteFooter />
</template>
