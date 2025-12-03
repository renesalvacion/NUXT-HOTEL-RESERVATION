<template>
  <div v-if="isOpen" class="fixed inset-0  bg-opacity-50 flex justify-center items-center z-50" style="background-color: rgba(0,0,0,0.5);">
    <div class="bg-white rounded-lg w-96 p-6 relative">
      <!-- Close Button -->
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-2 right-2 text-gray-600 hover:text-gray-800"
      >
         <span class="material-icons">close</span>
      </button>

      <h2 class="text-xl font-bold mb-4">Add Your Review</h2>

      <form @submit.prevent="submitReview" class="space-y-4">

        <!-- Room ID (Read-only) -->
       
            <input
                type="text"
                name="roomId"
                :value="props.roomId"
                class="w-full border border-gray-300 p-2 rounded bg-gray-100 cursor-not-allowed"
                hidden
            />

        <!-- Stars -->
        <div>
          <label class="block mb-1 font-medium">Stars (0-5)</label>
          <input
            type="number"
            name="stars"
            min="0"
            max="5"
            v-model.number="form.stars"
            class="w-full border border-gray-300 p-2 rounded"
            required
          />
        </div>

        <!-- Title -->
        <div>
          <label class="block mb-1 font-medium">Review Title</label>
          <input
            type="text"
            name="reviewTitle"
            v-model="form.reviewTitle"
            class="w-full border border-gray-300 p-2 rounded"
            required
          />
        </div>

        <!-- Message -->
        <div>
          <label class="block mb-1 font-medium">Review Message</label>
          <textarea
            name="reviewMessage"
            v-model="form.reviewMessage"
            rows="3"
            class="w-full border border-gray-300 p-2 rounded"
            required
          ></textarea>
        </div>

        <div class="flex justify-end space-x-2">
          <button
            type="button"
            @click="closeModal"
            class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="cursor-pointer px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Submit
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from "vue"
import { useReviewStore } from "~/stores/review"
import { useSessionStore } from "#imports"

const props = defineProps({
  isOpen: Boolean,
  roomId: Number
})

const emit = defineEmits(["close", "submitted"])

const form = reactive({
  stars: 0,
  reviewTitle: "",
  reviewMessage: "",
  roomId: props.roomId
})

const roomCatalogStore = useReviewStore()
const sessionStore = useSessionStore()

const closeModal = () => {
  emit("close")
}

// Submit review
const submitReview = async () => {
  const session = await sessionStore.getSession()
  if (!session?.id) {
    alert("You must be logged in to post a review")
    return
  }

  form.roomId = props.roomId
  
    await roomCatalogStore.PostReview(session.id, form)
}
</script>
