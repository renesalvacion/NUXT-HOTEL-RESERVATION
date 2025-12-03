import { defineStore } from 'pinia'
import axios from 'axios'
import { tr } from '@nuxt/ui/runtime/locale/index.js'
import { header } from '#build/ui'

export const useProfileStore = defineStore('profile', {
  state: () => ({
    profiles: [] as any[],
    data: null as any, 
         // To store API response
    error: '',    // To store any error messages
    loading: false
  }),

  

  actions: {
    async postProfile(payload:any) {
      try {
        const formData = new FormData()
        
        // Append data from the payload to FormData
        formData.append("name", payload.name)
        formData.append("email", payload.email)
        formData.append("password", payload.password)
        
        // If a profile file is provided, append it to the form
        if (payload.profile) {
          formData.append("profile", payload.profile)  // Expecting a file (e.g., image)
        }

        // Send POST request to the API
        const response = await axios.post("http://localhost:5080/api/Profile", formData, {
          headers: {
            'Content-Type': 'multipart/form-data',  // Ensures the API receives form data properly
          },
        })

        // Set the response data to the state
        this.data = response.data
      } catch (error:any) {
        // Handle any errors and store the error message
        this.error = error.response?.data || error.message
      }
    },


    async getData() {
      this.loading = true
      this.error = ''
      try {
        const response = await axios.get("http://localhost:5080/api/Profile")
        this.profiles = response.data
      } catch (error: any) {
        this.error = error.response?.data || error.message
      } finally {
        this.loading = false
      }
    },

async Login(payload: any) {
  this.loading = true;
  this.error = '';

  try {
    const response = await axios.post(
      "http://localhost:5080/api/Profile/login",
      payload,
      {
        headers: { 'Content-Type': 'application/json' }
      }
    );

    if (response.status === 200) {
      // Save token in localStorage or Pinia state
      const token = response.data.token;
      console.log("Login Function Axios: " + token);
      
      localStorage.setItem('token', token); // persist token
      this.data = response.data;            // optional: store user info
      this.loading = false;
      return true;
    } else {
      this.loading = false;
      return false;
    }
  } catch (error: any) {
    this.error = error.response?.data?.message || error.message;
    this.loading = false;
    return false;
  }
},

    async Logout() {
      this.loading = true
      this.error = ''

      try {
        const response = await axios.post(
          "http://localhost:5080/api/Profile/logout",
          {},
          { withCredentials: true }
        )

        if (response.status === 200) {
          this.loading = false
          return true
        } else {
          this.error = "Logout failed"
          this.loading = false
          return false
        }
      } catch (error: any) {
        this.error = error.message
        this.loading = false
        return false
      }
    },

    async ViewProfile(userId: number){
      try {
        const response = await axios.get(`http://localhost:5080/api/Profile/${userId}`)
        if(response.status === 200){
          return response.data
        }else{
          alert("Theres something wrong on the fetching.")
        }
      } catch (error:any) {
        alert("error: "+ error.Message)
      }
    }


  }
})
