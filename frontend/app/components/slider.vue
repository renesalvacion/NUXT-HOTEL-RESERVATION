<template>
  <br />
  <h1 class="font-bold text-2xl p-1">Available Reservation:</h1>
  <div class="cards pt-2 p-10">
    <swiper
      ref="swiper"
      :slides-per-view="1"
      :loop="true"
      :autoplay="{ delay: 3000 }"
      :keyboard="true"
      class="w-full"
      style="height: 100%;"
    >
      <swiper-slide v-for="(slideSet, index) in slides" :key="index">
        <div class=" flex justify-center w-full relative">
          <!-- Overlay (behind content, not on top) -->

          <!-- Image row -->
          <div class="flex gap-10 w-full justify-center relative z-10">
            <!-- Create a card for each image -->
            <div
              v-for="(slide, idx) in slideSet"
              :key="idx"
              class="bg-gray-50 shadow-xxl cursor-pointer card w-1/3 p-8"
              style="height: 100%;"
            >
              <img
                :src="slide"
                :alt="'Room Image ' + (idx + 1)"
                class=""
              />

              <!-- Buttons -->
              <div class="mt-3 flex flex-col gap-2">
                <button
                  @click="reserveRoom(idx)"
                  class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 rounded"
                >
                  Reserve
                </button>

                <button
                  @click="checkRoomDetails(idx)"
                  class="cursor-pointer bg-gray-800 hover:bg-gray-900 text-white font-bold py-2 rounded"
                >
                  Details
                </button>
              </div>
            </div>
          </div>
        </div>
      </swiper-slide>

      <!-- Custom Swiper Controls with Arrow SVGs -->
      <div class="swiper-pagination"></div>

      <!-- Custom Prev Arrow -->
      <div 
        class="swiper-button-prev flex items-center justify-center p-2 bg-white text-gray-600 rounded-full shadow-lg cursor-pointer"
        @click="goToPrevSlide"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
          <path fill="currentColor" d="M11.67 3.87L9.9 2.1L0 12l9.9 9.9l1.77-1.77L3.54 12z"/>
        </svg>
      </div>

      <!-- Custom Next Arrow -->
      <div 
        class="swiper-button-next flex items-center justify-center p-2 bg-white text-gray-600 rounded-full shadow-lg cursor-pointer"
        @click="goToNextSlide"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
          <path fill="currentColor" d="M6.23 20.23L8 22l10-10L8 2L6.23 3.77L14.46 12z"/>
        </svg>
      </div>
    </swiper>
  </div>
</template>

<script setup>
import { Swiper, SwiperSlide } from 'swiper/vue';
import { onMounted } from '#imports';
import 'swiper/css';
import 'swiper/css/navigation';
import 'swiper/css/pagination';

import { useRoomCatalogStore } from '#imports';  // Pinia Store

// Array of image sets, each slide contains multiple images
const slides = [
  [
    '/images/room.avif',
    '/images/room.avif',
    '/images/room.avif',
  ],
  [
    '/images/room.avif',
    '/images/room.avif',
    '/images/room.avif',
  ],
  [
    '/images/room.avif',
    '/images/room.avif',
    '/images/room.avif',
  ],
];

const roomCatalogStore = useRoomCatalogStore();

onMounted(() => {
  roomCatalogStore.GetCatalog(1, 10);
})

// Function to handle room reservation (this is just a placeholder)
const reserveRoom = (index) => {
  console.log(`Reservation button clicked for slide set ${index + 1}`);
  // Implement your reservation logic here (e.g., navigate to reservation page)
};

// Function to handle viewing room details
const checkRoomDetails = (index) => {
  console.log(`Check Details button clicked for slide set ${index + 1}`);
  // Implement logic to show detailed information about the room (e.g., navigate to room details page)
};
</script>

<style scoped>
/* Custom styles for the swiper */
.mySwiper {
  width: 100%;
  height: 100%;
}

.swiper-slide {
  display: flex;
  justify-content: center;
  align-items: center;
}

/* Style for the cards */
.card {
  width: 30%; /* Each card takes 30% of the width */
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
}

/* Hover effect on card */
.card:hover {
  transform: scale(1.05);
}

/* Style for the images inside cards */
img {
  width: 100%; /* Full width of the card */
  height: auto;
  object-fit: cover;
}

</style>
