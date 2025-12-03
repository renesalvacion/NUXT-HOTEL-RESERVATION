<template>
  <Navbar />
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-6">Reservation Details</h1>

    <div
      v-if="setAppDetails?.length"
      class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6"
    >
      <div
        v-for="app in setAppDetails"
        :key="app.appointmentId"
        class="bg-blue-50-100 shadow-md rounded-lg p-4 shadow-lg"
      >
        <h2 class="text-lg font-semibold mb-2">{{ app.crudsName }}</h2>
        <p><strong>Room Type:</strong> {{ app.roomType }}</p>
        <p><strong>Number of Beds:</strong> {{ app.numberBed }}</p>
        <p><strong>Number of Heads:</strong> {{ app.headPerson }}</p>
        <p><strong>Price:</strong> ${{ app.price }}</p>
        <p><strong>Status:</strong> {{ app.status || "N/A" }}</p>
        <p>
          <strong>Date:</strong> {{ new Date(app.created).toLocaleString() }}
        </p>
        <p><strong>Reservations:</strong> {{ app.reservationCount }}</p>

        <div class="btn flex justify-around ga-3 pt-5">
          <button
            @click="cancelModal(app.appointmentId)"
            class="cursor-pointer bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
          >
            Cancel Reservation
          </button>
          <button
            @click="adminEditModal(app.appointmentId)"
            class="cursor-pointer bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
          >
            Edit Reservation
          </button>
        </div>
      </div>
    </div>

    <div v-else class="text-gray-500">No reservation details found.</div>
  </div>

  <admin_cancel_modal
    v-if="isCancel"
    :appointment-id="appointmentIdToCancel || 0"
    @close="isCancel = false"
  />

  <user_edit_modal v-if="isEditAdmin" :appointment-id="appointmentIdToUpdate || 0" @close="isEditAdmin = false"/>
</template>
<script setup lang="ts">
    import Navbar from "~/components/navbar.vue";
    import { ref, onMounted } from "vue";
    import { SetAppointment } from "#imports";
    import { useRoute } from "vue-router";
    import admin_cancel_modal from "~/components/admin_cancel_modal.vue";

    import user_edit_modal from "~/components/admin_edit_modal.vue";

    const setApp = SetAppointment();
    const setAppDetails = ref<any[] | null>([]);

    const route = useRoute(); // <-- Get route params
    const userId = Number(route.params.id); // convert to number
    const appointmentIdToCancel = ref<number | null>(null);
    const appointmentIdToUpdate = ref<number | null>(null);

    const isCancel = ref(false);
    const isEditAdmin = ref(false);

    const cancelModal = (appointmentId: number) => {
        appointmentIdToCancel.value = appointmentId;
        isCancel.value = true;
    };

    const adminEditModal = (appointmentId: number) => {
      isEditAdmin.value = true;
      appointmentIdToUpdate.value = appointmentId
    }

    onMounted(async () => {
        if (!isNaN(userId)) {
            setAppDetails.value = await setApp.AdminViewUserAppointment(userId);
        } else {
            console.error("Invalid userId in route");
        }
    });
</script>
