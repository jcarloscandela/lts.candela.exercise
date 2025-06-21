<template>
  <v-dialog v-model="dialog" max-width="400" @update:model-value="onDialogUpdate">
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
  modelValue?: boolean
}>()

const emit = defineEmits<{
  (e: 'success', msg: string): void
  (e: 'error', msg: string): void
  (e: 'refresh'): void
  (e: 'close'): void
  (e: 'update:modelValue', value: boolean): void
}>()

const dialog = ref(props.modelValue ?? false)
const valid = ref(false)
const loading = ref(false)

watch(
  () => props.modelValue,
  (val) => {
    dialog.value = val ?? false
  }
)

watch(
  () => dialog.value,
  (val) => {
    emit('update:modelValue', val)
    if (!val) emit('close')
  }
)

function onDialogUpdate(val: boolean) {
  dialog.value = val
}
const form = ref()
const formData = ref({
  name: '',
  email: ''
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

/* openDialog removed: dialog is now controlled externally */

function closeDialog() {
  dialog.value = false
}

async function submit() {
  if (!(form.value as any).validate()) return
  loading.value = true
  try {
    if (props.mode === 'edit') {
      if (props.userData && 'id' in props.userData) {
        await userService.updateUser(props.userData.id, {
          name: formData.value.name,
          email: formData.value.email
        })
        emit('success', 'User updated successfully')
      } else {
        emit('error', 'User ID missing for update')
        return
      }
    } else {
      await userService.createUser({
        name: formData.value.name,
        email: formData.value.email,
        translationCredits: 0
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
