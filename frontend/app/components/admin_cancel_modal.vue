<template>
  <!-- Modal Overlay -->
  <div
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative"
    >
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>

      <h1 class="text-center font-semibold">Cancel Reservation?</h1>

      <form @submit.prevent="submitData">
        <input type="text" v-model="formData.status" hidden />

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
</template>

<script setup>
import { ref, defineProps, watch } from "vue";
import { SetAppointment } from "#imports";

const setApp = SetAppointment();
const emit = defineEmits(["close"]);

// Props from parent
const props = defineProps({
  appointmentId: Number,
});

// Reactive form data
const formData = ref({
  appointId: null,
  status: "Cancel",
});

// Watch for appointmentId prop changes
watch(
  () => props.appointmentId,
  (newId) => {
    formData.value.appointId = newId;
  },
  { immediate: true } // make sure it runs on initial render
);

// Submit function
const submitData = async () => {
  if (!formData.value.appointId) return;

  await setApp.AdminUpdateStatus(formData.value.appointId, {
    status: formData.value.status,
  });

  emit("close");
};

// Close modal
const closeModal = () => {
  emit("close");
};
</script>
