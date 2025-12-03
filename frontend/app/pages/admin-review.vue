<template>
    <NavBars/>
  <div class="p-6 pt-20">
    <h1 class="text-2xl font-bold mb-4">Admin View Review</h1>

    <div v-if="store.AdminReview && store.AdminReview.length > 0" class="overflow-x-auto">
      <table class="min-w-full bg-white border border-gray-200 rounded-lg">
        <thead class="bg-gray-100">
          <tr>
            <th class="px-4 py-2 border-b text-left">ID</th>
            <th class="px-4 py-2 border-b text-left">Title</th>
            <th class="px-4 py-2 border-b text-left">Stars</th>
            <th class="px-4 py-2 border-b text-left">Message</th>
            <th class="px-4 py-2 border-b text-left">Created At</th>
            <th class="px-4 py-2 border-b text-left">Room ID</th>
            <th  class="px-4 py-2 border-b text-left">Action</th>

          </tr>
        </thead>
        <tbody>
          <tr v-for="(review, index ) in store.AdminReview" :key="review.reviewId" class="hover:bg-gray-50">
            <td class="px-4 py-2 border-b">{{ index + 1}}</td>
            <td class="px-4 py-2 border-b">{{ review.reviewTitle }}</td>
            <td class="px-4 py-2 border-b">{{ review.stars }}</td>
            <td class="px-4 py-2 border-b">{{ review.reviewMessage }}</td>
            <td class="px-4 py-2 border-b">{{ new Date(review.reviewCreated).toLocaleString() }}</td>
            <td class="px-4 py-2 border-b">{{ review.roomId }}</td>
            <td class="py-2 px-4 border-b">
                <div class="btn flex gap-3">
                  <button
                    @click="viewSpecificReview(review.roomId)"
                    class="cursor-pointer bg-blue-400 text-white px-4 py-2 rounded hover:bg-blue-500 transition"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M12 16q1.875 0 3.188-1.312T16.5 11.5t-1.312-3.187T12 7T8.813 8.313T7.5 11.5t1.313 3.188T12 16m0-1.8q-1.125 0-1.912-.788T9.3 11.5t.788-1.912T12 8.8t1.913.788t.787 1.912t-.787 1.913T12 14.2m0 4.8q-3.65 0-6.65-2.037T1 11.5q1.35-3.425 4.35-5.462T12 4t6.65 2.038T23 11.5q-1.35 3.425-4.35 5.463T12 19m0-2q2.825 0 5.188-1.487T20.8 11.5q-1.25-2.525-3.613-4.012T12 6T6.813 7.488T3.2 11.5q1.25 2.525 3.613 4.013T12 17"
                      />
                    </svg>
                  </button>
                  
                </div>
              </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else class="text-gray-500 mt-4">
      <p>No reviews found.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useReviewStore } from "#imports";
import NavBars from "~/components/navbar.vue";
import { useRouter } from "vue-router";

const store = useReviewStore();
const router = useRouter()

const viewSpecificReview = (roomId: number) => {
    router.push(`/review-room/${roomId}`);
}
onMounted(async () => {
  await store.AdminViewReview();
});
</script>
