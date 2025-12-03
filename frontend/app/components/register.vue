<!--components/register.vue   note: this are modal-->

<template>
  <!-- Modal Overlay and Login Card -->
  <div v-if="isModalOpen" class="fixed inset-0  bg-opacity-50 flex justify-center items-center z-50" style="background-color: rgba(0,0,0,0.5);">
    
    <!-- Modal content -->
    <div class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60">
      <button 
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      
      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">REGISTER</h1>
      
      <form @submit.prevent="submitData" ref="registerForm" class="space-y-4">
        <!-- Name Input -->
        <div>
          <label for="name" class="block text-sm font-medium text-gray-700">Name:</label>
          <input 
            type="name" 
            id="name" 
            name="name"
            v-model="formData.name"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your email" 
            required
          />
        </div>

        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
          <input 
            type="email" 
            id="email" 
            name="email"
            v-model="formData.email"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your email" 
            required
          />
        </div>

        <!-- Password Input -->
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password:</label>
          <input 
            type="password" 
            id="password" 
            name="password"
            v-model="formData.password"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your password" 
            required
          />
        </div>

        <!-- Profile Input -->
        <div>
          <label for="profile" class="block text-sm font-medium text-gray-700">Profile:</label>
          <input 
            type="file" 
            id="profile" 
            name="profile"
             @change="handleFileUpload"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Upload your profile" 
            required
          />
        </div>

        <!-- Submit Button -->
        <div>
          <button 
            type="submit" 
            class="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2">
            Register
          </button>
        </div>
        
      </form>
      
      <!-- Footer / Sign up link -->
      <div class="text-center mt-4">
        <p class="text-sm text-gray-500">
            You have an account? <NuxtLink to="/modal">Log in here.</NuxtLink>
        </p>
      </div>
    </div>
  </div>

</template>

<script setup >
import {useProfileStore} from '~/stores/profile'
import { useRouter } from 'vue-router'; // Import useRouter
import { ref, defineProps } from 'vue'




  // Define emit
    const emit = defineEmits(['close', 'update:loading'])
    const router = useRouter();  // Create router instance
    const profileStores = useProfileStore()
    const profileImage = ref(null)

    // Track modal visibility state
    const isModalOpen = ref(true)
    const registerForm = ref<HTMLFormElement | null>(null)


    const props = defineProps({
        loading : Boolean,
        show : Boolean
    })

    const formData = ref({
        name : '',
        email:'',
        password:''
    })

    const handleFileUpload = (event) => {
        profileImage.value = event.target.files[0]
    }

    const resetForm = () => {
      // Reset reactive formData
      formData.value = {
        name: '',
        email: '',
        password: ''
      }

      // Reset the file input
      profileImage.value = null
      if (registerForm.value) {
        registerForm.value.reset()
      }
    }



    const submitData = async () => {
      if (!formData.value.email || !formData.value.password) {
          profileStores.error = 'Please fill in both email and password.'
          return
      }

      // Show spinner
      emit('update:loading', true)

      const payload = {
          name: formData.value.name,
          email: formData.value.email,
          password: formData.value.password,
          profile: profileImage.value
      }

      try {
          const success = await profileStores.postProfile(payload)
          
          if (success) {
              resetForm()
              closeModal()  // ✅ close modal AFTER success
          }

      } catch (err) {
          console.error('Register error', err)
      } finally {
          // Always hide spinner last
          emit('update:loading', false)
      }
    }


  // Close modal function
  const closeModal = () => {
    emit('close')
  }

</script>

<style scoped>
  /* Optional custom styling for the modal */
  .card {
    position: relative;
  }

  /* Optional: style for the close button */
  .material-icons {
    font-size: 24px;
  }

</style>
