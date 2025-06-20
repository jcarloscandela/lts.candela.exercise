<template>
  <v-dialog v-model="visible" max-width="400">
    <v-card>
      <v-card-title>
        Modify Credits
      </v-card-title>
      <v-card-text>
        <v-text-field
          v-model.number="credits"
          label="Credits"
          type="number"
          min="0"
          :rules="[v => v >= 0 || 'Must be non-negative']"
        />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="primary" @click="submit" :loading="loading">Save</v-btn>
        <v-btn text @click="close">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, defineEmits, defineProps } from 'vue'
import { updateUserCredits } from '../services/userService'

const props = defineProps<{
  userId: number | null
  initialCredits: number
  modelValue: boolean
}>()

const emits = defineEmits(['update:modelValue', 'updated'])

const visible = ref(props.modelValue)
const credits = ref(props.initialCredits)
const loading = ref(false)

watch(() => props.modelValue, v => visible.value = v)
watch(visible, v => emits('update:modelValue', v))
watch(() => props.initialCredits, v => credits.value = v)

function close() {
  visible.value = false
}

async function submit() {
  if (props.userId == null) return
  loading.value = true
  try {
    await updateUserCredits(props.userId, credits.value)
    emits('updated', credits.value)
    close()
  } catch (e) {
    alert('Failed to update credits')
  } finally {
    loading.value = false
  }
}
</script>
