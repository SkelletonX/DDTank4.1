<script setup lang="ts">
interface Props {
  open: boolean
  title?: string
}

defineProps<Props>()
const emit = defineEmits<{ close: [] }>()
</script>

<template>
  <div v-if="open" class="modal fade in" style="display: block;" role="dialog" aria-modal="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" aria-label="Close" @click="emit('close')">&times;</button>
          <p v-if="title" class="modal-title">{{ title }}</p>
        </div>
        <slot />
      </div>
    </div>
  </div>
  <div v-if="open" class="modal-backdrop fade in" @click="emit('close')"></div>
</template>
