<template>
  <v-container>
    <v-data-table-server
      v-model:items-per-page="itemsPerPage"
      :headers="headers"
      item-value="name"
      :items="serverItems"
      :items-length="totalItems"
      :loading="loading"
      @update:options="loadItems"
    ></v-data-table-server>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Email', value: 'email' },
  { title: 'Credits', value: 'translationCredits' },
]

const serverItems = ref([])
const totalItems = ref(0)
const loading = ref(false)
const itemsPerPage = ref(10)

async function loadItems (options: any) {
  loading.value = true
  try {
    const page = options.page || 1
    const pageSize = options.itemsPerPage || itemsPerPage.value
    const sortBy = options.sortBy?.[0]?.key || ''
    const sortOrder = options.sortBy?.[0]?.order === 'desc' ? 'desc' : 'asc'

    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    })
    if (sortBy) {
      params.append('sortBy', sortBy)
      params.append('sortOrder', sortOrder)
    }

    const response = await fetch(`https://localhost:7252/api/users?${params}`)
    const data = await response.json()
    serverItems.value = data.items || []
    totalItems.value = data.total || 0
  } catch {
    serverItems.value = []
    totalItems.value = 0
  } finally {
    loading.value = false
  }
}
</script>
