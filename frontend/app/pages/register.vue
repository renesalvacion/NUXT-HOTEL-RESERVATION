<template>
  
  <div class="flex justify-center items-center min-h-screen bg-gray-100">
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm"
    >
      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">
        Register
      </h1>

      <!-- Error Message -->
      <div v-if="profileStore.error" class="text-red-600 text-sm mb-4">
        <p>Error: {{ profileStore.error }}</p>
      </div>

      <!-- Success Message -->
      <div v-if="profileStore.data" class="text-green-600 text-sm mb-4">
        <p>Profile created successfully!</p>
      </div>

      <form @submit.prevent="submitProfile" class="space-y-4">
        <!-- Name Input -->
        <div>
          <label for="name" class="block text-sm font-medium text-gray-700"
            >Name:</label
          >
          <input
            type="text"
            v-model="formData.name"
            id="name"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your name"
            required
          />
        </div>

        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700"
            >Email:</label
          >
          <input
            type="email"
            v-model="formData.email"
            id="email"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your email"
            required
          />
        </div>

        <!-- Password Input -->
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700"
            >Password:</label
          >
          <input
            type="password"
            v-model="formData.password"
            id="password"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your password"
            required
          />
        </div>

        <!-- Profile Picture Input -->
        <div>
          <label for="profile" class="block text-sm font-medium text-gray-700"
            >Profile Picture:</label
          >
          <input
            type="file"
            @change="onFileChange"
            id="profile"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>

           <!-- Google Login Button -->
            <div class="mt-4">
                
                    <GoogleLoginButton
                        :verify-on-server="true"
                        :options="{ theme: 'filled_blue', size: 'large' }"
                        @success="onSuccess"
                        @verified="onVerified"
                        @error="onError"
                    />
            </div>

        <!-- Submit Button -->
        <div>
          <button
            type="submit"
            class="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
          >
            Register
          </button>
        </div>
      </form>

      <!-- Footer / Login link -->
      <div class="text-center mt-4">
        <p class="text-sm text-gray-500">
          Already have an account? <NuxtLink to="/login">Log in here.</NuxtLink>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useProfileStore } from "~/stores/profile";

// Create a reference for the form data
const formData = ref({
  name: "",
  email: "",
  password: "",
  profile: null, // This will hold the file
});

// Reactive states to show or hide messages
const showError = ref(false);
const showSuccess = ref(false);

// Access the store
const profileStore = useProfileStore();

// Handle file input change
const onFileChange = (event: any) => {
  formData.value.profile = event.target.files[0];
};

// Function to handle message visibility timeouts
const showMessage = (messageType:any) => {
  if (messageType === "error") {
    showError.value = true;
    setTimeout(() => {
      showError.value = false;
    }, 5000); // Message will disappear after 5 seconds
  } else if (messageType === "success") {
    showSuccess.value = true;
    setTimeout(() => {
      showSuccess.value = false;
    }, 5000); // Message will disappear after 5 seconds
  }
};

// Function to reset the form
const resetForm = () => {
  formData.value.name = "";
  formData.value.email = "";
  formData.value.password = "";
  formData.value.profile = null;
};

// Submit the profile data
const submitProfile = async () => {
  const payload = {
    name: formData.value.name,
    email: formData.value.email,
    password: formData.value.password,
    profile: formData.value.profile,
  };
  try {
    await profileStore.postProfile(payload);

    // Show the appropriate message based on the outcome
    if (profileStore.error) {
      showMessage("error"); // Show error message if there is an error
    } else if (profileStore.data) {
      showMessage("success"); // Show success message if profile is created successfully
      resetForm();
    }
  } catch (error) {
    alert(alert);
  }
};


 // eslint-disable-next-line no-console
const onSuccess = (e: { credential: string; claims: any }) => {
  console.log('success:', e.claims, e.credential.slice(0, 20) + '…');

  // Send the ID token to the backend for verification
  fetch('http://localhost:5080/api/Profile/User-SignIn-Google', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ IdToken: e.credential }), // Send the Google ID token
  })
  .then(response => response.json())
  .then(data => {
    // Handle the server response (e.g., store the JWT or user info)
    console.log('Backend response:', data);
    onVerified(data);
  })
  .catch(error => {
    console.error('Error sending to backend:', error);
  });
}
  // eslint-disable-next-line no-console
  const onVerified = (data: any) => {
    console.log('verified:', data)
  }
  // eslint-disable-next-line no-console
  const onError = (err: any) => {
    console.error('error:', err)
  }
</script>

<style scoped>
.error {
  color: red;
}
</style>
