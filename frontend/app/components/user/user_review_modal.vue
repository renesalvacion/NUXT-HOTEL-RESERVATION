<template>
  <!-- Modal Overlay and Login Card -->
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >

    <!-- Modal content -->
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60"
    >
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      <h1 class="text-center font-semibold">Review Your Room Reservation?</h1>


      <form @submit.prevent="" class="space-y-4">

        <!-- Room ID (Read-only) -->
       
            <input
                type="text"
                name="roomId"
                
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
            
            class="w-full border border-gray-300 p-2 rounded"
            required
          />
        </div>

        <!-- Message -->
        <div>
          <label class="block mb-1 font-medium">Review Message</label>
          <textarea
            name="reviewMessage"
            
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
import { ref, defineProps } from "vue";

// Define emit
const emit = defineEmits(["close", "update:loading"]);

// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
});

// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
