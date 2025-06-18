<template>
  <div class="min-h-screen bg-background">
    <main class="container mx-auto py-8">
      <router-view></router-view>
    </main>
  </div>
</template>

<script setup lang="ts">
// Main app component
</script>

<style>
@tailwind base;
@tailwind components;
@tailwind utilities;
 
@layer base {
  :root {
    --background: 0 0% 100%;
    --foreground: 222.2 47.4% 11.2%;
    --muted: 210 40% 96.1%;
    --muted-foreground: 215.4 16.3% 46.9%;
  }
 
  .dark {
    --background: 224 71% 4%;
    --foreground: 213 31% 91%;
    --muted: 223 47% 11%;
    --muted-foreground: 215.4 16.3% 56.9%;
  }
}
</style>
