import { defineStore } from 'pinia'
import axios from 'axios'


export const useSessionStore = defineStore('session', {
  state: () => ({
    session: null as { email?: string; role?: string; userId?: number } | null
  }),

  actions: {
    async getSession() {
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          console.log("No token found");
          this.session = null;
          return null;
        }

        const response = await axios.get(
          "http://localhost:5080/api/Profile/get-session",
          {
            headers: { Authorization: `Bearer ${token}` }
          }
        );
        // Safely extract role claim
        const role =
        response.data.role ||
        response.data["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
        null;
        
        // Store full session object
        this.session = {
          email: response.data.email,
          userId: response.data.userId,
          role
        };
        console.log("Session data:", this.session);
        console.log("Email:", this.session.email);
        console.log("User ID:", this.session.userId);
        console.log("Role:", this.session.role);
        console.log("Session data:", this.session);

        return this.session;
      } catch (error: any) {
        console.log("Error fetching session:", error.response?.data || error.message);
        this.session = null;
        return null;
      }
    }
  }
});
