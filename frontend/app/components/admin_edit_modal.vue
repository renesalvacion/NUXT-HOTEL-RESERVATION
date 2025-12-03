<template>
  <!-- Modal Overlay and Login Card -->
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >
    <h1>Edit Modal</h1>
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
      <h1 class="text-center font-semibold">Edit Reservation?</h1>

      <form @submit.prevent="submitEditReservation" class="space-y-4 bg-white p-6 rounded-lg shadow">

        <!-- Room Type -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            Room Type
          </label>
          <input
            type="text"
            v-model="formData.roomType"
            placeholder="Enter room type"
            class="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring focus:ring-indigo-200"
          />
        </div>

        <!-- Head Per Person -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            Head Per Person
          </label>
          <input
            type="number"
            v-model="formData.headPerson"
            placeholder="Enter head count"
            class="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring focus:ring-indigo-200"
          />
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          class="w-full bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700 transition"
        >
          Update Reservation
        </button>

      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, onMounted, watch } from "vue";
import { SetAppointment } from "#imports";
import { useSessionStore } from "#imports";
// Define emit
const emit = defineEmits(["close", "update:loading"]);

const setApp = SetAppointment();
const sessionStore = useSessionStore();

const session = ref(null);
// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  appointmentId: Number,
});

// Local reactive copy of appointmentId
const appId = ref(props.appointmentId ?? 0);

// Directly use props.appointmentId
console.log(props.appointmentId); // Access appointmentId directly

// Define formData as a reactive object for the form fields
const formData = ref({
  reservationId: appId.value, // Initialize with appId
  roomType: '',
  headPerson: 0,
});

// Watch appId and update formData when it changes
watch(appId, (newAppId) => {
  formData.value.reservationId = newAppId;
});

const submitEditReservation = async () => {
  if (!session.value) {
    console.error("Session is not loaded yet");
    return; // Prevent submitting if session is not loaded
  }

  console.log("Session ID:", session.value.userId);
  
  // Ensure the payload is correctly formatted
  const payload = {
    reservationId: formData.value.reservationId,
    roomType: formData.value.roomType,
    headPerPerson: formData.value.headPerson,
  };

  console.log("Payload being sent:", payload);  // Log the payload for debugging
  
  // Call the API with the correctly formatted payload
  await setApp.AdminUpdateReservation(session.value.userId, payload);
}; 


onMounted(async () => {
  session.value = await sessionStore.getSession();
  // You can check if the session is loaded
  console.log("Session Loaded:", session.value);
});

// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
