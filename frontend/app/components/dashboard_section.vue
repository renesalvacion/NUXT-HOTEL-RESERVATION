<template>
  <div class="grid grid-cols-1 gap-4 w-full p-3">
    <h3 class="text-3xl font-semibold">Available Rooms:</h3>

    <!-- Loop through rooms in the Pinia store -->
    <div
      class="card border-t-gray-50 rounded-lg shadow-xxl shadow-md overflow-hidden"
      v-for="(hotel, idx) in roomCatalogStore.catalog.rooms"
      :key="hotel?.roomId || idx"
    >
      <div class="flex" style="height: fit-content;">
        <!-- Image -->
        <img
          :src="`http://localhost:5080/roomImage/${hotel.room.roomImage}`"
          alt="Hotel Image"
          class="w-96 h-60 object-cover shrink-0"
        />

        <!-- Text container -->
        <div class="flex flex-col justify-center p-4 h-60 gap-3">
          <h3 class="text-2xl font-semibold">Name: {{ hotel.room.name }}</h3>
          <p class="text-gray-500">Descriptions: {{ hotel.room.description }}</p>
          <p class="text-gray-500">Max Occupancy: {{ hotel.room.maxOccupancy }}</p>

          <button
            class="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 rounded mt-2 w-40"
            @click="ViewDetails(hotel.room.roomId)"
          >
            Check Details
          </button>
        </div>

        <!-- Right section: ratings / price / button -->
        <div class="flex flex-col justify-between items-center ml-auto p-3 h-60 gap-2">
          <div class="text-center">
            <p class="text-green-500">Excellent</p>
            <button
              class="bg-green-100 p-1 font-bold rounded mt-1"
            >
              {{hotel.averageStars.toFixed(1)}}
            </button>
          </div>

          <h5 class="text-2xl font-bold">${{ hotel.room.price }}</h5>

          <button
            @click="reserveRoom(idx)"
            class="text-black p-3 bg-blue-200 hover:bg-blue-400 font-bold rounded w-40"
          >
            Start Booking
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal for Room Booking -->
  <appointment_modal
    v-if="showModal"
    @close="closeModal"
    :room="selectedRoom" 
  />
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoomCatalogStore } from '#imports';  // Pinia Store
import { SetAppointment } from '../stores/appointment';  // Store for appointment modal
import appointment_modal from './appointment_modal.vue';
import { useRouter } from "vue-router";

const showModal = ref(false);
const selectedRoom = ref(null);  // Keep track of the selected room

// Room catalog store and data initialization
const store = SetAppointment();
const roomCatalogStore = useRoomCatalogStore();

const router = useRouter()
// Fetch rooms from the store on mounted
onMounted(() => {
  roomCatalogStore.GetCatalog(1, 10);
});

// Reserve Room Handler
function reserveRoom(idx) {
  const selected = roomCatalogStore.catalog.rooms[idx];
  selectedRoom.value = selected;  // Set the selected room
  store.setSelectedRoom(selected);
  showModal.value = true;  // Open modal
}

// Close modal handler
function closeModal() {
  showModal.value = false;
}

const ViewDetails = (roomId) => {
  router.push(`/room-catalog/${roomId}`);
};
</script>
