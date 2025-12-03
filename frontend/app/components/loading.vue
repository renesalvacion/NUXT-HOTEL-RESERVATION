<template>
  <!-- Full-screen overlay -->
  <div v-if="show" class="fixed inset-0 flex items-center justify-center z-50 ">
    <!-- Card container -->
    <div class="relative w-80 h-52 bg-white rounded-lg shadow-md flex flex-col items-center justify-center overflow-hidden">
      <!-- Spinner inside card -->
      <div class="w-12 h-12 border-4 border-gray-200 border-t-blue-500 rounded-full animate-spin"></div>
        <p>loading...</p>
    </div>

    <div class="content pt-3">
      <h4>{{ title }}</h4>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps({
  show: Boolean,
  title : String
})
</script>
