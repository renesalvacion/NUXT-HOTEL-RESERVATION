<!--pages/catalog.vue-->

<template>
  <Navbar />
  <div
    class="max-w-4xl mx-auto mt-10 p-20 bg-white shadow-md rounded-lg pt-15"
    v-if="session?.role === 'Admin'"
  >
    <h2 class="text-2xl font-bold mb-6 text-gray-800">Add New Room</h2>

    <form
      @submit.prevent="submitRoomCatalog"
      class="grid grid-cols-1 md:grid-cols-2 gap-6"
    >
      <!-- Room Name -->
      <div>
        <label for="name" class="block mb-1 font-medium text-gray-700"
          >Room Name</label
        >
        <input
          type="text"
          id="name"
          name="name"
          placeholder="Enter room name"
          v-model="formData.name"
          required
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Room Number -->
      <div>
        <label for="roomnumber" class="block mb-1 font-medium text-gray-700"
          >Room Number</label
        >
        <input
          type="text"
          id="roomnumber"
          name="roomnumber"
          placeholder="Enter room number"
          required
          v-model="formData.roomnumber"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Description -->
      <div class="md:col-span-2">
        <label for="description" class="block mb-1 font-medium text-gray-700"
          >Description</label
        >
        <textarea
          required
          v-model="formData.description"
          id="description"
          name="description"
          rows="2"
          placeholder="Enter room description"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        ></textarea>
      </div>

      <!-- Price -->
      <div>
        <label for="price" class="block mb-1 font-medium text-gray-700"
          >Price</label
        >
        <input
          required
          v-model="formData.price"
          type="number"
          id="price"
          name="price"
          placeholder="Enter price"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Room Type -->
      <div>
        <label for="price" class="block mb-1 font-medium text-gray-700"
          >Room Type</label
        >
        <input
          required
          v-model="formData.roomType"
          type="text"
          id="roomType"
          name="roomType"
          placeholder="Enter price"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Max Occupancy -->
      <div>
        <label for="maxoccupancy" class="block mb-1 font-medium text-gray-700"
          >Max Occupancy</label
        >
        <input
          required
          v-model="formData.maxoccupancy"
          type="number"
          id="maxoccupancy"
          name="maxoccupancy"
          placeholder="Max guests"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Room Status -->
      <div>
        <label for="roomstatus" class="block mb-1 font-medium text-gray-700"
          >Room Status</label
        >
        <select
          required
          v-model="formData.roomstatus"
          id="roomstatus"
          name="roomstatus"
          class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="">Select status</option>
          <option value="Available">Available</option>
          <option value="Booked">Booked</option>
          <option value="Maintenance">Maintenance</option>
        </select>
      </div>

      <!-- Room Image -->
      <div>
        <label for="roomimage" class="block mb-1 font-medium text-gray-700"
          >Room Image</label
        >
        <input
          @change="onFileChange"
          type="file"
          id="roomimage"
          name="roomimage"
          class="cursor-pointer w-full border border-gray-300 p-1 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>

      <!-- Submit Button (span full width) -->
      <div class="md:col-span-2 pt-">
        <button
          type="submit"
          class="w-full bg-blue-600 text-white p-2 rounded-md hover:bg-blue-700 transition duration-200"
        >
          Add Room
        </button>
      </div>
    </form>
  </div>

  <ViewCatalog />
</template>

<script setup>
import { useRoomCatalogStore } from "~/stores/room_catalog";
import { useSessionStore } from "#imports";
import NavBar from "~/components/navbar.vue";
import ViewCatalog from "../components/view_catalog.vue";

const roomCatalogStore = useRoomCatalogStore();
const rooms = ref([]);

const sessionStore = useSessionStore();
const session = ref(null);

const loading = ref(true);

// Create a reference for the form data
const formData = ref({
  name: "",
  description: "",
  price: "",
  maxoccupancy: "",
  roomnumber: "",
  roomstatus: "",
  roomType: "",
  roomimage: null, // This will hold the file
});

// Handle file input change
const onFileChange = (event) => {
  formData.value.roomimage = event.target.files[0];
};

const submitRoomCatalog = async () => {
  try {
    const payload = new FormData();
    payload.append("name", formData.value.name);
    payload.append("description", formData.value.description);
    payload.append("price", formData.value.price);
    payload.append("maxOccupancy", formData.value.maxoccupancy);
    payload.append("roomNumber", formData.value.roomnumber);
    payload.append("roomStatus", formData.value.roomstatus);
    payload.append("roomType", formData.value.roomType)
    if (formData.value.roomimage) {
      payload.append("roomImage", formData.value.roomimage);
    }

    const response = await roomCatalogStore.CreateCatalog(payload);
    console.log("Room catalog response:", response);

    // Optional: reset form
    formData.value = {
      name: "",
      description: "",
      price: "",
      maxoccupancy: "",
      roomnumber: "",
      roomstatus: "",
      roomimage: null,
    };
  } catch (error) {
    console.error("Error adding room:", error);
  }
};

onMounted(async () => {
  const result = await roomCatalogStore.GetCatalog();
  rooms.value = roomCatalogStore.catalog.rooms || [];

  // Fetch session
  session.value = await sessionStore.getSession();
  
  console.log("Session:", session.value); // Check role here
});
</script>
