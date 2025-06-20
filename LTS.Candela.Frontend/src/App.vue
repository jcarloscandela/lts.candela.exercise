<template>
<v-app id="inspire">
  <v-navigation-drawer v-model="drawer">
    <v-list>
      <v-list-item router to="/users">

        <v-list-item-title> <v-icon>mdi-account-group</v-icon> Users </v-list-item-title>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>

    <v-app-bar>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-app-bar-title>Application</v-app-bar-title>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
  import { ref } from 'vue'

  const drawer = ref(true)
</script>
