<script setup lang="ts">
import { ref } from 'vue'

const activeTab = ref(0)
const tabs = ['Hot', 'Noticias', 'Eventos', 'Anùncios']
</script>

<template>
  <div class="news mt20 clearfix">
    <div class="news-box f-l">
      <div class="news-tab clearfix">
        <span v-for="(tab, index) in tabs" :key="tab" :class="{ select: activeTab === index }" @click="activeTab = index">
          <cite></cite>{{ tab }}
        </span>
        <a class="news-more f-r" href="#" target="_blank">Ver Mais</a>
      </div>
      <div class="clearfix">
        <div v-show="activeTab === 0" class="news-content">
          <ul class="news-ul clearfix">
            <li class="first">
              <h2 class="clearfix"><a target="_blank" href="#">Uma nova jornada nos aguarda!</a></h2>
              <p class="clearfix">
                A segurança sempre em primeiro lugar. Sempre coloque uma senha em sua mochila para manter a sua conta mais segura!
                <a target="_blank" class="show fn-right" href="/Web/Assets/gamedata/detail/49.html">Ver mais</a>
              </p>
            </li>
            <li><span>Hot</span><a href="#" target="_blank">Em Desenvolvimento</a><em>[17:03]</em></li>
            <li><span>Hot</span><a href="#" target="_blank">Servidor s1</a><em>[17:41]</em></li>
          </ul>
        </div>
        <div v-for="index in [1, 2, 3]" :key="index" v-show="activeTab === index" class="news-content">
          <ul class="news-ul clearfix">
            <li class="first">
              <h2 class="clearfix"><a target="_blank" href="#">Em Desenvolvimento</a></h2>
              <p class="clearfix">Em Desenvolvimento <a target="_blank" class="show fn-right" href="#">Ver detalhes&gt;&gt;</a></p>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
