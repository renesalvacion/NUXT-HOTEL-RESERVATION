<template>
  <NavBar/>

  <div class="main-div pt-30 p-10">
      <h1 class="text-center font-bold text-3xl">View Review : </h1>

  <div class="get pt-5" v-if="store.ViewSpecificReviewArray?.length">
    <!-- Grid container with 4 columns -->
    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
      <div
        class="card bg-white shadow-lg p-4 break-words"
        v-for="(rev, index) in store.ViewSpecificReviewArray"
        :key="index"
      >
        <h2 class="text-lg font-semibold mb-2">{{ index + 1 }}. {{ rev.reviewTitle }}</h2>
        <p class="mb-2 break-words">{{ rev.reviewMessage }}</p>
        <p class="mb-2">Stars: {{ rev.stars }}</p>
        <small class="text-gray-500">Created: {{ new Date(rev.reviewCreated).toLocaleString() }}</small>
      </div>
    </div>
  </div>


    <div v-else class="text-gray-500">
      No reviews found.
    </div>
  </div>
</template>


<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useReviewStore } from '#imports'
import NavBar from '~/components/navbar.vue'

const store = useReviewStore()
const route = useRoute()
const roomId = Number(route.params.id)

console.log("RoomId:", roomId)

onMounted(async () => {
  if (!isNaN(roomId)) {
    await store.ViewSpecificReview(roomId) // store.ViewSpecificReviewArray will be updated
    console.log("Fetched reviews:", store.ViewSpecificReviewArray)
  } else {
    console.error("Invalid roomId in route:", route.params)
  }
})
</script>
