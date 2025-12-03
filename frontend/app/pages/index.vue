<!--index.vue-->

<template>

  <header :style="headerStyle">
    <div class="overlay"></div>

    <div
      class="section_t flex justify-between items-center"
      style="position: absolute; width: 100%; height: 100%"
    >
      <div
        class="text_h flex flex-col items-center p-5"
        style="line-height: 4rem"
      >
        <div class="s_h text-center">
          <h1 class="text-white text-3xl font-bold">LOREM IPSUM LOREM IPSUM</h1>
          <h5 class="text-white">
            Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum
          </h5>
        </div>

        <div class="btn pt-2 flex gap-4">
          <button
            type="button"
            class="cursor-pointer rounded text-body text-white bg-neutral-secondary-medium box-border border border-default-medium hover:bg-neutral-tertiary-medium hover:text-heading focus:ring-4 focus:ring-neutral-tertiary shadow-xs font-medium leading-5 rounded-base text-sm px-4 py-2.5 focus:outline-none"
            @click="openModal"
          >
            LOGIN
          </button>
          <button
            type="button"
            class="cursor-pointer rounded text-body text-white bg-neutral-secondary-medium box-border border border-default-medium hover:bg-neutral-tertiary-medium hover:text-heading focus:ring-4 focus:ring-neutral-tertiary shadow-xs font-medium leading-5 rounded-base text-sm px-4 py-2.5 focus:outline-none"
          >
            Discover
          </button>
        </div>
      </div>

      <div class="second_t p-5" >
        <h1 class="text-white text-center text-4xl font-bold text-shadow">
          Book Your Dream Stay at Our Luxury Hotel
        </h1>
        <div class="pt-4" style="line-height: 2rem;">
          <p class="text-white text-center text-shadow">
            Escape to comfort and luxury with our exclusive hotel offers. <br> Experience the best in hospitality, designed to make your stay unforgettable.
          </p>
          <p class="text-white text-center text-shadow">
            Enjoy world-class amenities, spacious rooms, and personalized services—all at unbeatable prices.
          </p>
          <p class="text-white text-center text-shadow">
            <strong>Book now and save up to 30% on your next stay!</strong>
          </p>
        </div>
    </div>

    </div>
  </header>

  <section class="p-10">
    <slider />
  </section>

  <SavedModal
    v-if="isOpen"
    :loading="loading"
    @update:loading="loading = $event"
    @close="isOpen = false"
  />

  <loadingSpinner :show="loading"/>

  <SectionH />
  <contact />


</template>

<script setup lang="ts">
import navbar from "~/components/navbar.vue";
import slider from "~/components/slider.vue";
import SavedModal from "~/components/modal.vue";
import { ref } from "vue";


import headerImage from "~/assets/images/hotel_header.avif";
import SectionH from "~/components/section.vue";
import contact from "~/components/contact.vue";
import loadingSpinner from "~/components/loading.vue";

const isOpen = ref(false);
const loading = ref(false);

const headerStyle = `
  background-image: url(${headerImage});
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat; 
  height: 40rem;
  
  position: relative;
`;

// For testing, keep spinner always running
// To hide it after 3 seconds:
setTimeout(() => {
  loading.value = false;
}, 3000);

const openModal = () => {
  isOpen.value = true;
};
</script>

<style scoped>
.overlay {
  position: absolute;
  inset: 0; /* top:0; right:0; bottom:0; left:0 */
  background-color: rgba(0, 0, 0, 0.4); /* semi-transparent dark background */
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
