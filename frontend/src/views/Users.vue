<template>
  <div>
    <h1 class="text-3xl font-bold mb-8">Users</h1>
    
    <div class="rounded-md border">
      <table class="w-full">
        <thead>
          <tr class="border-b bg-muted">
            <th class="p-4 text-left font-medium">Name</th>
            <th class="p-4 text-left font-medium">Email</th>
            <th class="p-4 text-left font-medium">Translation Credits</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id" class="border-b">
            <td class="p-4">{{ user.name }}</td>
            <td class="p-4">{{ user.email }}</td>
            <td class="p-4">{{ user.translationCredits }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'

interface User {
  id: string
  name: string
  email: string
  translationCredits: number
}

const users = ref<User[]>([])

const fetchUsers = async () => {
  try {
    const response = await axios.get<User[]>('/api/user')
    users.value = response.data
  } catch (error) {
    console.error('Error fetching users:', error)
  }
}

onMounted(() => {
  fetchUsers()
})
</script>
