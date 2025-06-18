<template>
  <div>
    <h1 class="text-3xl font-bold mb-8">Users</h1>
    
    <div class="card">
      <DataTable
        :value="users"
        :paginator="true"
        :rows="5"
        :rowsPerPageOptions="[5, 10, 20, 50]"
        v-model:first="first"
        :totalRecords="totalRecords"
        :loading="loading"
        tableStyle="min-width: 50rem"
        @page="onPage($event)"
      >
        <Column field="name" header="Name" style="width: 30%"></Column>
        <Column field="email" header="Email" style="width: 40%"></Column>
        <Column field="translationCredits" header="Translation Credits" style="width: 30%"></Column>
      </DataTable>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { UserService } from '../services/UserService';
import type { User } from '../types/User';

const users = ref<User[]>([]);
const loading = ref(true);
const first = ref(0);
const totalRecords = ref(0);

const fetchUsers = async (page: number = 1) => {
  try {
    loading.value = true;
    const pageSize = 5;
    const response = await UserService.getUsers({ page, pageSize });
    users.value = response.items;
    totalRecords.value = response.totalItems;
  } catch (error) {
    console.error('Error fetching users:', error);
  } finally {
    loading.value = false;
  }
};

const onPage = (event: { page: number, first: number, rows: number }) => {
  first.value = event.first;
  fetchUsers(event.page + 1);
};

onMounted(() => {
  fetchUsers();
});
</script>
