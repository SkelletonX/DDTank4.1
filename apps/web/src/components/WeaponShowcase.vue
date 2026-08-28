<script setup lang="ts">
import { ref } from 'vue'

type Item = { src: string; alt: string; name: string }
type Category = { name: string; items: Item[] }

const activeCategory = ref(0)

const categories: Category[] = [
  { name: 'Armas', items: [
    ['0001.png','Bumerangue Do Amors','Bumerangue Do Amor'],['0002.png','Lança da Antiguidade','Lança da Antiguidade'],['0003.png','Cabeça De Boi','Cabeça De Boi'],['0004.png','Super - Quebra Tijolos','Super - Quebra Tijolos'],['0005.png','Super - Fogo Intenso','Super - Fogo Intenso'],['0006.png','Super - Kit Medico','Super - Kit Medico']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/Armas/${src}`,alt,name })) },
  { name: 'Chapeu', items: [
    ['0001.png','Primavera Verde','Primavera Verde'],['0002.png','Chapéu do Barão','Chapéu do Barão'],['0003.png','Chapéu do Bugou','Chapéu do Bugou'],['0004.png','Ouvidos de lobo','Ouvidos de lobo'],['0005.png','Pequeno Gorro do demônio','Pequeno Gorro'],['0006.png','Arrependimento Demônio das Trevas','Arrependimento']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/Chapeu/${src}`,alt,name })) },
  { name: 'Roupas', items: [
    ['0001.png','Armadura de Matias','Roupa de Matias'],['0002.png','Raiva de Solaan','Raiva de Solaan'],['0003.png','Guerreiro Demônio','Guerreiro'],['0004.png','Capa de Matias','Capa de Matias'],['0005.png','Mano','Mano'],['0006.png','Bravo Mago','Bravo Mago']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/roupas/${src}`,alt,name })) },
  { name: 'asas', items: [
    ['0001.png','Asas do Amor Assistente','Asas do Amor'],['0002.png','Lâmpada de Aladim','Lâmpada Aladim'],['0003.png','Bola Colorida do Parque de Diversão','Bola Colorida'],['0004.png','Previsão de tempo','Deus da Guerra'],['0005.png','Encantador e fascinante','Previsão de tempo'],['0006.png','Encantador e fascinante','Encantadar']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/asas/${src}`,alt,name })) },
  { name: 'Ternos', items: [
    ['0001.png','Bugou das Profundezas do Mar','Bugou do Mar'],['0002.png','Leão(M)','Leão(M)'],['0003.png','Libra dourada','Libra dourada'],['0004.png','Dama de doce coração','Top 1 Hall'],['0005.png','Uniforme do líder das estudantes (Feminino)','Estudantes'],['0006.png','Bela Princesa Fênix','Bela Princesa']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/Ternos/${src}`,alt,name })) },
  { name: 'Acessórios', items: [
    ['0001.png','Pedra de fortalecimento nível 1','Pedra nível 1'],['0002.png','Pedra de fortalecimento nível 2','Pedra nível 2'],['0003.png','Pedra de fortalecimento nível 3','Pedra nível 3'],['0004.png','Pedra de fortalecimento nível 4','Pedra nível 4'],['0005.png','Pedra de fortalecimento nível 5','Pedra nível 5'],['0006.png','Símbolo dos Deuses','Símbolo']
  ].map(([src,alt,name]) => ({ src:`/Web/Assets/images/img/Acess%c3%b3rios/${src}`,alt,name })) },
]
</script>

<template>
  <div class="weapon mt10">
    <div class="box-tit weapon-tit">
      <a class="box-more f-r" href="/Web/Assets/main.html" target="_blank">Mais</a>
      <strong>Arma de estimação</strong>
    </div>
    <div class="weapon-box">
      <div class="weapon-menu p-r clearfix">
        <span v-for="(category, index) in categories" :key="category.name" :class="{ select: activeCategory === index }" @click="activeCategory = index">
          {{ category.name }}
        </span>
        <div class="tab-underline p-a"></div>
      </div>
      <div class="clearfix">
        <ul class="weapon-ul clearfix">
          <li v-for="item in categories[activeCategory].items" :key="item.src + item.name">
            <a target="_blank" href="#">
              <img :alt="item.alt" :src="item.src" />
              <span>{{ item.name }}</span>
            </a>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
