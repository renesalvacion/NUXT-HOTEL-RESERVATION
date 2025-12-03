<template>
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >
    <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md relative">
      <button
        @click="closeModal"
        class="absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>

      <h1 class="text-center text-lg font-semibold mb-4">Reservation Form</h1>

      <form @submit.prevent="submitForm" class="space-y-4">
        <div>
          <label class="block mb-1">Room Type</label>
          <input
            type="text"
            v-model="formData.roomType"
            name="roomType"
            class="w-full border rounded px-3 py-2"
            placeholder="Enter room type"
          />
        </div>

        <div>
          <label class="block mb-1">Number of Beds</label>
          <input
            type="number"
            name="numberBed"
            v-model.number="formData.numberBed"
            class="w-full border rounded px-3 py-2"
            min="1"
          />
        </div>

        <div>
          <label class="block mb-1">Head Person</label>
          <input
            type="number"
            name="headPerson"
            v-model.number="formData.headPerson"
            class="w-full border rounded px-3 py-2"
            min="1"
          />
        </div>



        <div>
          <label class="block mb-1">Reservation Date</label>
          <input
            type="date"
            name="reservationDate"
            v-model="formData.reservationDate"
            class="w-full border rounded px-3 py-2"
          />
        </div>
        <input type="text" v-model="localAppId" hidden name="appointmentId">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white rounded py-2 hover:bg-blue-700 transition"
        >
          Submit
        </button>
        
      </form>

    </div>
  </div>

  <h1>userID{{ sessionId?.userId }}</h1>
</template>

<script setup>
import { ref, defineProps, onMounted } from "vue";
import { SetAppointment } from "#imports";
import { useSessionStore } from "#imports";
// Define emit
const emit = defineEmits(["close", "update:loading"]);

const setApp = SetAppointment()
const session = useSessionStore()

// Track modal visibility state
const isModalOpen = ref(true);
const sessionId = ref(null)

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  appointmentId: Number,
});

const localAppId = ref(props.appointmentId)
const formData = ref({
    roomType : '',
    numberBed : 0,
    headPerson : 0, 
    reservationDate : ''
})


const submitForm = async () => {
  if (!sessionId.value?.userId) {
    alert("User session not loaded yet.");
    return;
  }

  const payload = {
    appointmentId: localAppId.value,
    roomType : formData.value.roomType,
    numberBed : formData.value.numberBed,
    headPerson : formData.value.headPerson,
    reservationDate : formData.value.reservationDate
  }
  console.log("user id: "+sessionId.value?.userId);
  
  // Send the inner value of the ref
  console.log("userId in Submit: " + sessionId.value?.userId);
  
  await setApp.UserUpdateReservation(sessionId.value?.userId, payload);
};


onMounted(async() => {
  
  sessionId.value = await session.getSession()
  console.log("OnMount: " + sessionId.value?.userId);
  
})

// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
