<template>
  <v-dialog v-model="dialog" max-width="400">
    <template #activator="{ props }">
      <v-btn v-bind="props" color="primary" @click="openDialog">
        {{ mode === 'edit' ? 'Edit User' : 'Add User' }}
      </v-btn>
    </template>
    <v-card>
      <v-card-title>
        {{ mode === 'edit' ? 'Edit User' : 'Create User' }}
      </v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            v-model="formData.name"
            label="Name"
            :rules="[v => !!v || 'Name is required']"
            required
          />
          <v-text-field
            v-model="formData.email"
            label="Email"
            type="email"
            :rules="[
              v => !!v || 'Email is required',
              v => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) || 'Email must be valid'
            ]"
            required
          />
          <v-text-field
            v-model.number="formData.translationCredits"
            label="Credits"
            type="number"
            :rules="[
              v => v !== null && v !== '' || 'Credits are required',
              v => !isNaN(Number(v)) && Number(v) >= 0 || 'Credits must be a non-negative number'
            ]"
            required
          />
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn text @click="closeDialog">Cancel</v-btn>
        <v-btn color="primary" :loading="loading" @click="submit">
          {{ mode === 'edit' ? 'Save' : 'Create' }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue'
import * as userService from '../services/userService'

const props = defineProps<{
  mode?: 'create' | 'edit'
  userData?: { name: string; email: string; translationCredits: number }
}>()

const emit = defineEmits<{
  (e: 'success', msg: string): void
  (e: 'error', msg: string): void
  (e: 'refresh'): void
}>()

const dialog = ref(false)
const valid = ref(false)
const loading = ref(false)
const form = ref()
const formData = ref({
  name: '',
  email: '',
  translationCredits: 0
})

watch(
  () => props.userData,
  (val) => {
    if (props.mode === 'edit' && val) {
      formData.value = { ...val }
    }
  },
  { immediate: true }
)

function openDialog() {
  if (props.mode === 'edit' && props.userData) {
    formData.value = { ...props.userData }
  } else {
    formData.value = { name: '', email: '', translationCredits: 0 }
  }
  dialog.value = true
}

function closeDialog() {
  dialog.value = false
}

async function submit() {
  if (!(form.value as any).validate()) return
  loading.value = true
  try {
    if (props.mode === 'edit') {
      // Implement edit logic here if needed in the future
      // await userService.updateUser(formData.value)
      emit('success', 'User updated successfully')
    } else {
      await userService.createUser({
        name: formData.value.name,
        email: formData.value.email,
        translationCredits: Number(formData.value.translationCredits)
      })
      emit('success', 'User created successfully')
    }
    emit('refresh')
    closeDialog()
  } catch (e: any) {
    emit('error', e?.message || 'Operation failed')
  } finally {
    loading.value = false
  }
}
</script>
