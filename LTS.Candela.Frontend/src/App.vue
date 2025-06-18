<template>
  <div class="min-h-screen bg-background">
    <Toast />
    <main class="container mx-auto py-8">
      <router-view></router-view>
    </main>
  </div>
</template>

<script setup lang="ts">
import Toast from 'primevue/toast';
</script>

<style>

</style>
