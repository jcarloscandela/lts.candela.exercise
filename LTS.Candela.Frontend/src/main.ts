import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import Users from './views/Users.vue'
import './style.css'
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/users' },
    { path: '/users', component: Users }
  ]
})


const app = createApp(App);
app.use(router);
app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});

app.mount('#app')
