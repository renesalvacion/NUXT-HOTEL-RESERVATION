<template>
  <div class="mt-10 p-4">
    <h2 class="text-2xl font-bold mb-4 p-3">Room Catalog</h2>

    <div v-if="rooms.length === 0">
      No rooms available.
    </div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-5 gap-6">
      <div
        v-for="room in rooms"
        :key="room.roomId"
        class="bg-white cursor-pointer border-gray-300 rounded-lg shadow-lg hover:shadow-lg transition p-4 flex flex-col hover:scale-103"
      >
        <img
          v-if="room.room.roomImage"
          :src="`http://localhost:5080/RoomImage/${room.room.roomImage}`"
          alt="Room Image"
          class="w-full h-auto object-cover rounded"
          loading="lazy"
        />

        <h3 class="font-semibold text-lg mb-1">{{ room.room.name }}</h3>
        <p class="text-gray-600 text-sm mb-1">{{ room.room.description }}</p>
        <p class="text-gray-800 font-medium mb-1">Price: ${{ room.room.price }}</p>
        <p class="text-gray-800 font-medium mb-1">Max Occupancy: {{ room.room.maxOccupancy }}</p>
        <p class="text-gray-800 font-medium">Status: {{ room.room.roomStatus }}</p>

        <div class="btn flex justify-around pt-2 mt-auto">
          <button
            class="cursor-pointer bg-fuchsia-400 hover:bg-fuchsia-700 text-white font-semibold py-1 px-3 rounded shadow-md hover:shadow-lg transition duration-200 ease-in-out text-sm"
          >
            Add Room
          </button>
          <button
            @click="viewRoomDetails(room.room.roomId)"
            class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white font-semibold py-1 px-3 rounded shadow-md hover:shadow-lg transition duration-200 ease-in-out text-sm"
          >
            Check Details
          </button>
        </div>
      </div>
    </div>

    <!-- Pagination -->
<div class="flex justify-center mt-6 space-x-2 flex-wrap">
  <!-- Prev button -->
  <button
    @click="prevPage"
    :disabled="pageNumber === 1"
    class="cursor-pointer   px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
  >
    Prev
  </button>

        <!-- Page number buttons -->
        <button
            v-for="page in totalPages"
            :key="page"
            @click="goToPage(page)"
            :class="[
            'cursor-pointer px-4 py-2 rounded hover:bg-gray-400',
            pageNumber === page ? 'bg-blue-600 text-white' : 'bg-gray-300'
            ]"
        >
            {{ page }}
        </button>

        <!-- Next button -->
        <button
            @click="nextPage"
            :disabled="pageNumber === totalPages"
            class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
        >
            Next
        </button>
        </div>

  </div>
</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useRoomCatalogStore } from '~/stores/room_catalog'

const roomCatalogStore = useRoomCatalogStore()
const rooms = ref([])
const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0) // total number of rooms from backend
const router = useRouter()

const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value))

const fetchRooms = async () => {
  const result = await roomCatalogStore.GetCatalog(pageNumber.value, pageSize.value);
  rooms.value = roomCatalogStore.catalog.rooms || [];
  totalCount.value = roomCatalogStore.catalog.totalCount || rooms.value.length;

  // Check if pagination is working as expected
  console.log('Fetched rooms:', rooms.value);
  console.log('Total rooms:', totalCount.value);
  console.log('Current page:', pageNumber.value);
  console.log('Total pages:', totalPages.value);
};


const nextPage = () => {
  if (pageNumber.value < totalPages.value) pageNumber.value++
}
const prevPage = () => {
  if (pageNumber.value > 1) pageNumber.value--
}
const goToPage = (page) => {
  pageNumber.value = page
}

watch(pageNumber, () => {
  fetchRooms()
})

onMounted(() => {
  fetchRooms()
})

const viewRoomDetails = (roomId) => {
  router.push(`/room-catalog/${roomId}`)
}

</script>
