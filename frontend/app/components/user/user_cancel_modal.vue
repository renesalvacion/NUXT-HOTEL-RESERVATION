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
      <h1 class="text-center font-semibold">Are You Sure Reservation?</h1>

      <form @submit.prevent="cancelReserv(session?.id )">
        <div class="btn flex justify-around pt-10">
          <button
          
            type="submit"
            class="cursor-pointer bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
          >
            Cancel Reservation
          </button>
          <button
            type="button"
            @click="closeModal"
            class="cursor-pointer bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
          >
            Close
          </button>
        </div>
      </form>
      
    </div>
  </div>

  <h1>{{ session?.id }}</h1>
  <h1>App ID: {{ localAppId }}</h1>
</template>

<script setup>
import { ref, defineProps, onMounted } from "vue";
import { SetAppointment } from "#imports";
import { useSessionStore } from "#imports";



// Define emit
const emit = defineEmits(["close", "update:loading"]);
const session = ref(null)
const setApp = SetAppointment()
const sessionStore = useSessionStore()

// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  appointmentId: Number,
});

const localAppId = ref(props.appointmentId)


const cancelReserv = async (userId) => {
  if (!localAppId.value) {
    alert("No appointment ID found!");
    return;
  }

  await setApp.cancelReservation(userId, localAppId.value);
  emit("close"); // close modal after cancelling
}


onMounted(async () => {
  session.value = await sessionStore.getSession();
})

// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
