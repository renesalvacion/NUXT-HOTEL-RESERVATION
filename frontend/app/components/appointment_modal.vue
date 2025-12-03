<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { SetAppointment } from '~/stores/appointment'
import { useSessionStore } from '#imports'

const emit = defineEmits(['close', 'update:loading'])
const store = SetAppointment()
const sessionStore = useSessionStore()
const session = ref<{id:number; username:string} | null>(null)

// Reactive form data
const formData = ref({
  status : '',
  name :  '',
  roomType: '',
  numberBed: null,
  headerPerson: null,
  price: 0
})

// Reactive selected room from store
const selectedRoom = computed(() => store.selectedRoom)

// Initialize formData when modal opens
if (selectedRoom.value) {
  formData.value.roomType = selectedRoom.value.type
  formData.value.price = selectedRoom.value.price
}

// Close modal function
const closeModal = () => {
  emit('close')
}

// Submit appointment function
const submitData = async () => {
  if (!formData.value.numberBed || !formData.value.headerPerson) {
    alert('Please fill all fields')
    return
  }

  if(!session.value?.id){
    alert("You must logged in to set an appoitment")
    return
  }

  emit('update:loading', true)

  const payload = {
    status: formData.value.status,
    roomType: formData.value.roomType,
    numberBed: formData.value.numberBed,
    headerPerson: formData.value.headerPerson,
    price: formData.value.price
  }

  try {
    const success = await store.SetApp(payload, session.value.id) // pass userId if needed
    emit('update:loading', false)

    if (success ) {
      alert('Appointment set successfully')
      closeModal()
    } else {
      alert('Failed to set appointment')
    }
  } catch (err) {
    emit('update:loading', false)
    console.error(err)
    alert('An error occurred')
  }
}

onMounted(async () => {
  session.value = await sessionStore.getSession()
})
</script>

<template>
  <!-- Modal overlay -->
 <div
  class="fixed inset-0 flex items-center justify-center z-50"
  style="background: rgba(0, 0, 0, 0.7);"
>

    <!-- Modal box -->
    <div
      class="bg-white rounded-lg w-96 p-6 relative shadow-lg transform transition duration-300 scale-100"
    >
      <!-- Close button -->
      <button
        class="absolute top-2 right-2 text-gray-500 hover:text-gray-700 text-xl"
        @click="closeModal"
      >
        ✖
      </button>

      <form v-if="selectedRoom" class="space-y-4">
        <input type="text" v-model="formData.status" hidden>
        <!-- Room Name -->
        <div>
          <label class="block font-semibold mb-1">Room:</label>
          <input
            type="text"
            class="w-full border rounded px-3 py-2"
            :value="selectedRoom.name"
            readonly
          />
        </div>

        <!-- Room Type -->
        <div>
          <label class="block font-semibold mb-1">Type:</label>
          <input
            type="text"
            class="w-full border rounded px-3 py-2"
            v-model="formData.roomType"
            readonly
          />
        </div>

        <!-- Bed Type -->
        <div>
          <label class="block font-semibold mb-1">Bed:</label>
          <input
            type="text"
            class="w-full border rounded px-3 py-2"
            :value="selectedRoom.bed"
            readonly
          />
        </div>

        <!-- Number of Bed -->
        <div>
          <label class="block font-semibold mb-1">Number of Bed:</label>
          <input
            type="number"
            class="w-full border rounded px-3 py-2"
            v-model.number="formData.numberBed"
          />
        </div>

        <!-- Room per head -->
        <div>
          <label class="block font-semibold mb-1">Room per Head:</label>
          <input
            type="number"
            class="w-full border rounded px-3 py-2"
            v-model.number="formData.headerPerson"
          />
        </div>

        <!-- Price -->
        <div>
          <label class="block font-semibold mb-1">Price:</label>
          <input
            type="text"
            class="w-full border rounded px-3 py-2"
            :value="selectedRoom.price"
            readonly
          />
        </div>

        <!-- Room image -->
        <img
          :src="`http://localhost:5080/RoomImage/${selectedRoom.roomImage}`"
          alt="Room Image"
          class="w-full h-40 object-cover rounded mt-2"
        />

        <!-- Centered submit button -->
        <div class="flex justify-center mt-4">
          <button
            type="button"
            @click="submitData"
            class="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-6 rounded shadow-md transition duration-300 transform hover:scale-105"
          >
            Set Appointment
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
