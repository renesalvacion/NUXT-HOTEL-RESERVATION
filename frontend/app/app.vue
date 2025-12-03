<template>
  <link
    href="https://fonts.googleapis.com/icon?family=Material+Icons"
    rel="stylesheet"
  />

  <!-- Page wrapper -->
  <div class="min-h-screen flex flex-col">

    <!-- Main content -->
    <main class="flex-grow">
      <NuxtPage />
    </main>

    <!-- Footer always at bottom -->
    <Footers style="margin-top:5rem;"/>
  </div>
</template>

<script setup>
import Footers from "./components/footer.vue";
</script>
