<template>

  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="card bg-white rounded-lg shadow-lg p-6 w-full max-w-sm">

      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">Login</h1>

      <!-- Login Form -->
      <form @submit.prevent="submitData" class="space-y-4">
        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
          <input 
            type="email" 
            id="email" 
            name="email"
            v-model="formData.email"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
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
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your password" 
            required
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
            class="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 focus:outline-none">
            Log In
          </button>
        </div>
      </form>



      <!-- Footer -->
      <div class="text-center mt-4">
        <p class="text-sm text-gray-500">
          Don't have an account? <NuxtLink to="/register">Sign here.</NuxtLink>
        </p>
      </div>
    </div>
  </div>
</template>



<script setup lang="ts">
import { Icon } from '@iconify/vue';
import { useProfileStore } from '~/stores/profile';
import { useRouter } from 'vue-router';
import { ref } from 'vue';


// Use profile store for login actions
const profileStores = useProfileStore();
const router = useRouter();

// Define the form data for binding input fields
const formData = ref({
  email: '',
  password: ''
});

// Method for handling login form submission
const submitData = async () => {
  if (!formData.value.email || !formData.value.password) {
    profileStores.error = 'Please fill in both email and password.';
    return;
  }

  try {
    // Update loading state (use store or local state as needed)
    profileStores.error = ''; // Clear previous errors

    const success = await profileStores.Login(formData.value);
    if (success) {
      router.push("/dashboard");
    } else {
      profileStores.error = 'Invalid credentials.';
    }
  } catch (err) {
    profileStores.error = 'An error occurred. Try again.';
  }
}

const onSuccess = (e: any) => {
  console.log('success:', e.claims, e.credential.slice(0, 20) + '…');

  // Send the ID token to the backend for verification
  fetch('http://localhost:5080/api/Profile/Google-Login-Google', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ IdToken: e.credential }), // Send the Google ID token
  })
  .then(response => response.json()) // Parse the JSON response
  .then(data => {
    // Check if the response contains the expected properties (e.g., message and token)
    if (data.message && data.token) {
      const token = data.token;  // Assuming 'data.token' is the JWT token returned from the backend
      console.log('Login Function: ' + token);

      // Save token in localStorage or Pinia state (if using state management)
      localStorage.setItem('token', token);  // Persist token in localStorage

      // Redirect to the dashboard
      router.push("/dashboard"); // This should redirect to the dashboard
    } else {
      console.error('Login failed:', data.message);  // Handle error if any
    }
  })
  .catch(error => {
    console.error('Error sending to backend:', error);
    alert('An error occurred during login. Please try again.');
  });
};

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
/* Optional: Style adjustments */
.material-icons { font-size: 24px; }
.card { position: relative; }
</style>
