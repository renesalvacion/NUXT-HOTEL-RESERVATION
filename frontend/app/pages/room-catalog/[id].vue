<!--pages/room-catalog/[id].vue-->

<template>
    <NavBar/>
    <div class="min-h-screen flex items-center justify-center bg-gray-100 p-4">
    <div v-if="details" class="max-w-4xl w-full bg-white rounded-lg shadow-lg p-6 space-y-4">
        <img 
            v-if="details.roomImage" 
            :src="`http://localhost:5080/RoomImage/${details.roomImage}`" 
            alt="Room Image" 
            class="w-full h-80 object-cover rounded-md"
        />

        <h1 class="text-center text-2xl font-bold text-gray-800">{{ details.name }}</h1>
        <p class="text-gray-600">{{ details.description }}</p>
        <div class="flex justify-between text-gray-800 font-medium">
            <p>Price: ${{ details.price }}</p>
            <p>Max Occupancy: {{ details.maxOccupancy }}</p>
        </div>
        <div class="flex justify-between text-gray-800 font-medium">
            <p>Room Number: {{ details.roomNumber }}</p>
            <p>Status: {{ details.roomStatus }}</p>
        </div>

    <div class="btn flex justify-between">
        <button class="cursor-pointer bg-fuchsia-400 hover:bg-fuchsia-700 
                            text-white font-semibold py-1 px-3 rounded shadow-md hover:shadow-lg 
                            transition duration-200 ease-in-out text-sm">
                    Add Room
        </button>
        <button @click="openModal" class="cursor-pointer bg-blue-400 hover:bg-fuchsia-700 
                            text-white font-semibold py-1 px-3 rounded shadow-md hover:shadow-lg 
                            transition duration-200 ease-in-out text-sm">
                    Review
        </button>
        <div class="rev font-semibold flex gap-2">
            <p>Review:</p>
            <button class="cursor-pointer bg-green-400 hover:bg-green-700 
                            text-white font-semibold py-1 px-3 rounded shadow-md hover:shadow-lg 
                            transition duration-200 ease-in-out text-sm">
                    {{ details.averageStars }}
            </button>


        </div>
        
    </div>

    </div>

    <div v-else class="text-gray-500 text-lg">
      Loading room details...
    </div>
  </div>

  <review_modal
    :isOpen="modalOpen"
    :roomId="selectedRoomId"
    @close="modalOpen = false"/>


</template>


<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useRoomCatalogStore } from '~/stores/room_catalog'
import NavBar from '~/components/navbar.vue'
import review_modal from '~/components/review_modal.vue'

const route = useRoute()
const roomId = Number(route.params.id)
const catalog = useRoomCatalogStore()
const details = ref(null)
const selectedRoomId = ref(null)

const modalOpen = ref(false)

const openModal = () => {
    selectedRoomId.value = roomId 
    modalOpen.value = true
}

onMounted(async () => {
  details.value = await catalog.GetDetailsRoom(roomId)
  console.log('Fetched room details:', details.value)
})
</script>

