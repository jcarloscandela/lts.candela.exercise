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
    >
      <template #item.actions="{ item }">
        <v-btn color="red" icon @click="openDeleteDialog(item)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table-server>
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Confirm Delete</v-card-title>
        <v-card-text>
          Are you sure you want to delete this user?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="deleteDialog = false">Cancel</v-btn>
          <v-btn color="red" text @click="deleteUser">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-snackbar v-model="snackbar" :color="snackbarColor" timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const deleteDialog = ref(false)
const userToDelete = ref<any>(null)
const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

function openDeleteDialog(user: any) {
  userToDelete.value = user
  deleteDialog.value = true
}

async function deleteUser() {
  if (!userToDelete.value) return
  try {
    const response = await fetch(`https://localhost:7252/api/users/${userToDelete.value.id}`, {
      method: 'DELETE'
    })
    if (!response.ok) throw new Error('Delete failed')
    snackbarText.value = 'User deleted successfully'
    snackbarColor.value = 'success'
    await loadItems({ page: 1, itemsPerPage: itemsPerPage.value })
  } catch {
    snackbarText.value = 'Error deleting user'
    snackbarColor.value = 'error'
  } finally {
    snackbar.value = true
    deleteDialog.value = false
    userToDelete.value = null
  }
}

const headers = [
  { title: 'Name', value: 'name' },
  { title: 'Email', value: 'email' },
  { title: 'Credits', value: 'translationCredits' },
  { title: 'Actions', value: 'actions', sortable: false },
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
