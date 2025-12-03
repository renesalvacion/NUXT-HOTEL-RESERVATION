<script  setup lang="ts">
import { ref, onMounted, computed } from "vue";
import register from "./register.vue";
import modal from "./modal.vue";
import loadingSpinner from "./loading.vue";
import { useSessionStore, useProfileStore } from "#imports";
import { useRouter } from "vue-router";



interface SessionType {
  email?: string;
  role?: string;
  userId?: number;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
}
const session = ref<SessionType | null>(null);

const isOpen = ref(false);
const isOpenLogin = ref(false)
const loading = ref(false);
const mobileMenu = ref(false);

const sessionStore = useSessionStore();
const profileStore = useProfileStore();
const router = useRouter();

const openModal = () => (isOpen.value = true);
const loginModal = () => (isOpenLogin.value = true)
const closeModal = () => (isOpen.value = false);
const sessionEmail = computed(() => session.value?.email);
const sessionRole = computed(() => session.value?.role);
const userId = computed(() => session.value?.userId);


const logout = async () => {
  const success = await profileStore.Logout();
  if (success) {
    sessionStore.session = null;
    session.value = null; // clear local session ref
    localStorage.removeItem("token");
    router.push("/");
  } else {
    alert("Logout failed.");
  }
};

onMounted(async () => {
  const s = await sessionStore.getSession();
  console.log("S:", s);           // Logs the object itself (recommended)
console.log("S: " + JSON.stringify(s, null, 2)); // Pretty-printed JSON

  session.value = s;

    if (!session.value) {
    router.push("/login");
  }
});

</script>


<template>
  <nav class="backdrop-blur-md bg-black/60 shadow-lg fixed top-0 left-0 w-full z-50 ">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16 items-center">
        <!-- LOGO -->
        <div class="flex items-center">
          <a href="/" class="flex items-center gap-2">
            <img src="https://tailwindcss.com/plus-assets/img/logos/mark.svg" alt="Logo" class="h-8 w-auto drop-shadow-lg" />
            <span class="text-white font-semibold text-lg tracking-wide">HotelEase</span>
          </a>
        </div>

<div class="hidden sm:flex space-x-6 text-sm font-medium">
    <template v-if="session?.email">
      <NuxtLink to="/dashboard" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Dashboard
      </NuxtLink>

      <NuxtLink to="/reservation" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Reservations
      </NuxtLink>

      <NuxtLink to="/catalog" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Catalog
      </NuxtLink>

      <NuxtLink to="/admin-review" class="nav-item text-white" v-if="session.role === 'Admin'">
        Admin-Catalog
      </NuxtLink>

 
    </template>
</div>



  <div class="hidden sm:flex items-center gap-4">
  <template v-if="session">
<span class="text-white text-sm">Hi, {{ sessionEmail }}</span>
<span class="text-white text-sm">Role: {{ sessionRole }}</span>



    <NuxtLink
      to="/profile"
      class="px-4 py-2 text-sm rounded-md bg-gray-700 hover:bg-gray-600 text-white transition"
    >
      Profile
    </NuxtLink>

    <button
      @click="logout"
      class="px-4 py-2 text-sm rounded-md bg-red-500 hover:bg-red-600 text-white transition"
    >
      Logout
    </button>
  </template>

  <template v-else>
    <NuxtLink
      to="/login"
      class="px-4 py-2 text-sm rounded-md bg-blue-600 hover:bg-blue-700 text-white transition"
    >
      Login
    </NuxtLink>

    <button
      @click="openModal"
      class="px-4 py-2 text-sm rounded-md bg-green-600 hover:bg-green-700 text-white transition"
    >
      Sign Up
    </button>
  </template>
</div>


        <!-- MOBILE MENU BUTTON -->
        <button
          @click="mobileMenu = !mobileMenu"
          class="sm:hidden text-white p-2 rounded hover:bg-white/10 transition"
        >
          <span class="material-icons text-3xl">{{ mobileMenu ? "close" : "menu" }}</span>
        </button>
      </div>
    </div>

    <!-- MOBILE MENU -->
    <div v-if="mobileMenu" class="sm:hidden bg-black/80 backdrop-blur-md text-white px-4 py-4 space-y-3">
      <template v-if="session">
        <NuxtLink to="/dashboard" class="mobile-item">Dashboard</NuxtLink>
        <NuxtLink to="/reservation" class="mobile-item">Reservations</NuxtLink>
        <NuxtLink to="/catalog" class="mobile-item">Catalog</NuxtLink>
        <NuxtLink to="/admin-catalog" class="mobile-item">Admin-Catalog</NuxtLink>
        <NuxtLink to="/profile" class="mobile-item">Profile</NuxtLink>

        
        <button @click="logout" class="mobile-item text-red-300">Logout</button>
      </template>

      <template v-else>
        <NuxtLink to="/" class="mobile-item">Home</NuxtLink>
        <NuxtLink to="/about" class="mobile-item">About</NuxtLink>
        <NuxtLink to="/contact" class="mobile-item">Contact</NuxtLink>
        <NuxtLink to="/login" class="mobile-item">Login</NuxtLink>
        <button @click="openModal" class="mobile-item text-green-300">Sign Up</button>
      </template>
    </div>
  </nav>

  <!-- SIGN UP MODAL -->
  <register v-if="isOpen" :loading="loading" @update:loading="loading = $event" @close="isOpen = false" />
  <loadingSpinner :show="loading" />
</template>
