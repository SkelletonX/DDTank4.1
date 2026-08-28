<script setup lang="ts">
import { ref } from 'vue'

const slides = [
  { src: '/Web/Assets/images/slider/20000.jpg', alt: 'A volta das Old' },
  { src: '/Web/Assets/images/slider/200001.jpg', alt: 'O Retorno do labolatorio' },
  { src: '/Web/Assets/images/slider/200002.jpg', alt: 'A melhor versão está de volta' },
  { src: '/Web/Assets/images/slider/200003.jpg', alt: 'Uma nova jornada nos Aguarda' },
]

const titles = [
  'DDtank Na Sua Melhor Forma!',
  'O Retorno do labolatorio!',
  'Com Parceiros Fica Mais Divertido!',
  'Uma nova jornada nos Aguarda!',
]

const activeIndex = ref(0)
</script>

<template>
  <div class="focus clearfix">
    <div class="focus-img f-l">
      <a
        v-for="(slide, index) in slides"
        :key="slide.src"
        target="_blank"
        title=""
        :class="{ hidden: index !== activeIndex }"
      >
        <img :alt="slide.alt" width="503" height="200" :src="slide.src" />
      </a>
    </div>
    <div class="focus-menu f-r">
      <span
        v-for="(title, index) in titles"
        :key="title"
        :class="{ select: index === activeIndex }"
        @click="activeIndex = index"
      >{{ title }}</span>
    </div>
  </div>
</template>
