<template>
  <Navbar />
  <header>
    <swiper
      class="h-[600px] relative"
      :modules="[Autoplay, Pagination, Navigation]"
      :loop="true"
      :autoplay="{ delay: 3000 }"
      pagination
      navigation
    >
      <swiper-slide v-for="(slide, index) in slides" :key="index">
        <div
          class="h-[600px] bg-cover bg-center relative"
          :style="{ backgroundImage: `url(${slide.image})` }"
        >
          <div class="overlay"></div>

          <!-- Centered Caption -->
          <div
            class="caption flex flex-col items-center justify-center text-center"
          >
            <h1>{{ slide.title }}</h1>
            <p>{{ slide.subtitle }}</p>

            <!-- Button only on first slide -->
            <button
              v-if="index === 0"
              class="mt-6 px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
            >
              Get Started
            </button>
          </div>
        </div>
      </swiper-slide>
    </swiper>
  </header>

  <section class="flex">
    <dashboardSection />
  </section>
</template>

<script setup>
import Navbar from "~/components/navbar.vue";
import dashboardSection from "~/components/dashboard_section.vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, Pagination, Navigation } from "swiper/modules";
import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";
import { useRouter } from "vue-router";
import { onMounted, ref } from "vue";
import { useSessionStore } from "#imports";
import { useRoomCatalogStore } from "../stores/room_catalog";


const router = useRouter();
const sessionStore = useSessionStore();
const slides = ref([
  {
    image: "/images/contact.avif",
    title: "Welcome to Our Site",
    subtitle: "Explore the best experiences",
  },
  {
    image: "/images/header.jpg",
    title: "Luxury Rooms",
    subtitle: "Comfort and elegance combined",
  },
  {
    image: "/images/room.avif",
    title: "Book Your Stay",
    subtitle: "Reserve your favorite room today",
  },
]);

const roomCatalogStore = useRoomCatalogStore();
const rooms = ref([]);


onMounted(() => {
  rooms.value = roomCatalogStore.catalog.rooms || [];
})
</script>

<style>
.overlay {
  position: absolute;
  inset: 0;
  background-color: rgba(0, 0, 0, 0.4);
}

.caption {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(
    -50%,
    -50%
  ); /* center both horizontally and vertically */
  color: #fff;
  z-index: 10;
}

.caption h1 {
  font-size: 4rem; /* bigger title */
  font-weight: bold;
}

.caption p {
  font-size: 1.5rem; /* bigger subtitle */
  margin-top: 1rem;
}

.caption button {
  font-size: 1.2rem;
}
</style>
