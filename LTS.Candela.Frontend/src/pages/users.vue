<template>
  <v-container>
    <UserDialog
      mode="create"
      @success="onUserDialogSuccess"
      @error="onUserDialogError"
      @refresh="onUserDialogRefresh"
    />
    <UserDialog
      v-model="editDialogVisible"
      mode="edit"
      :userData="editDialogUser"
      @success="onUserDialogSuccess"
      @error="onUserDialogError"
      @refresh="onUserDialogRefresh"
      @close="onEditDialogClose"
    />
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
        <v-btn color="primary" icon @click="openCreditsDialog(item)">
          <v-icon>mdi-currency-usd</v-icon>
        </v-btn>
        <v-btn color="blue" icon @click="openEditDialog(item)">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>
        <v-btn color="red" icon @click="openDeleteDialog(item)">
          <v-icon>mdi-delete</v-icon>
        </v-btn>
      </template>
    </v-data-table-server>
    <v-skeleton-loader
      v-if="loading && serverItems.length === 0"
      class="my-4"
      :height="300"
      :loading="loading"
      type="table"
    />
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
    <UserCreditsDialog
      v-if="selectedUser"
      v-model="showCreditsDialog"
      :user-id="selectedUser.id"
      :initial-credits="selectedUser.translationCredits"
      @updated="onCreditsUpdated"
    />
  </v-container>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { deleteUserById, fetchUsers } from '../services/userService'
  import UserDialog from '../components/UserDialog.vue'
  import UserCreditsDialog from '../components/UserCreditsDialog.vue'

  const deleteDialog = ref(false)
  const userToDelete = ref<any>(null)
  const snackbar = ref(false)
  const snackbarText = ref('')
  const snackbarColor = ref('success')

  const showCreditsDialog = ref(false)
  const selectedUser = ref<any>(null)
  const editDialogUser = ref<any>(null)
  const editDialogVisible = ref(false)

  function openEditDialog(user: any) {
    editDialogUser.value = user
    editDialogVisible.value = true
  }

  function onEditDialogClose() {
    editDialogVisible.value = false
    editDialogUser.value = null
  }

  function openCreditsDialog(user: any) {
    selectedUser.value = user
    showCreditsDialog.value = true
  }

  function onCreditsUpdated(newCredits: number) {
    snackbarText.value = 'Credits updated'
    snackbarColor.value = 'success'
    snackbar.value = true
    showCreditsDialog.value = false
    selectedUser.value = null
    loadItems({ page: currentPage.value, itemsPerPage: itemsPerPage.value })
  }

  function openDeleteDialog (user: any) {
    userToDelete.value = user
    deleteDialog.value = true
  }

  async function deleteUser () {
    if (!userToDelete.value) return
    try {
      await deleteUserById(userToDelete.value.id)
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
  const currentPage = ref(1)
  const totalPages = ref(1)
  const loading = ref(false)
  const itemsPerPage = ref(10)

  async function loadItems (options: any) {
    loading.value = true
    try {
      const page = options.page || 1
      const pageSize = options.itemsPerPage || itemsPerPage.value
      const sortBy = options.sortBy?.[0]?.key || ''
      const sortOrder = options.sortBy?.[0]?.order === 'desc' ? 'desc' : 'asc'

      const data = await fetchUsers(page, pageSize, sortBy, sortOrder)
      serverItems.value = data.items || []
      totalItems.value = data.totalItems || 0
      currentPage.value = data.currentPage || 1
      totalPages.value = data.totalPages || 1
    } catch {
      serverItems.value = []
      totalItems.value = 0
      currentPage.value = 1
      totalPages.value = 1
    } finally {
      loading.value = false
    }
  }

  function onUserDialogSuccess(msg: string) {
    snackbarText.value = msg
    snackbarColor.value = 'success'
    snackbar.value = true
  }

  function onUserDialogError(msg: string) {
    snackbarText.value = msg
    snackbarColor.value = 'error'
    snackbar.value = true
  }

  function onUserDialogRefresh() {
    loadItems({ page: currentPage.value, itemsPerPage: itemsPerPage.value })
  }
</script>
