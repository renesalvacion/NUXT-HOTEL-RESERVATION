import { defineStore } from 'pinia'
import axios from 'axios'
import { useSessionStore } from '#imports'
import { tr } from '@nuxt/ui/runtime/locale/index.js'
import { user } from '#build/ui'


export const SetAppointment = defineStore('SetAppointment', {
  state: () => ({
    loading: false,
    message: '',
    error: null as string | null,
    selectedRoom: null as any | null,
    Appointments : [] as any [],
    AdminViewAppointment : [] as any [],
    totalCount: 0,
    pageNumber: 1,
    pageSize: 10,
    totalPages: 1,
    adminApp : [] as any []

  }),

  actions: {
    setSelectedRoom(room: any) {
      this.selectedRoom = room
    },

    async SetApp(payload: any, userId: number) {
      try {
        this.loading = true
        
        const response = await axios.post(
          `http://localhost:5080/api/Appointment/${userId}`,
          payload,
          {
            withCredentials: true,
            headers: { 'Content-Type': 'application/json' }
          }
        )

        this.loading = false

        if (response.status === 200) {
          this.message = 'Set Appointment Successfully'
          return true
        } else {
          this.error = 'Failed to set appointment'
          return false
        }
      } catch (err: any) {
        this.loading = false
        this.error = err.message
        return false
      }
    },

    async getAppointment(userId : number) {
      try {

        const response = await axios.get(`http://localhost:5080/api/Appointment/${userId}`)

        this.Appointments = response.data.appointments || []
        console.log(this.Appointments)
      } catch (error: any) {
        alert(error.message || 'Failed to fetch appointments')
        this.Appointments = []
      }
    },


  async ViewAdminCatalog(pageNumber = 1, pageSize = 10) {
    try {
      const res = await axios.get(
        `http://localhost:5080/api/Appointment/view-appointment-admin?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        { withCredentials: true }
      );

      console.log("Backend response:", res.data); // Debugging response

      const appointments = res.data.appointments || [];
;
      console.log("Appointments data:", appointments); // Log appointments data

      this.AdminViewAppointment = appointments;
      this.pageNumber = pageNumber;
      this.pageSize = pageSize;
      this.totalCount = res.data.totalCount || appointments.length;
      this.totalPages = Math.ceil(this.totalCount / this.pageSize);

    } catch (err) {
      console.error("Error fetching paginated appointments:", err);
    }
  },



    async AdminViewUserAppointment(userId: number) {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(`http://localhost:5080/api/Appointment/view-appointment-admin/${userId}`, {
          headers:{Authorization: `Bearer ${token}`},withCredentials : true
        })

        if (response.status === 200) {
          this.adminApp = response.data.results
          console.log("Viewe Admin Appointment: " + response.data.results);
          
          return this.adminApp
        } else {
          this.adminApp = []
          return this.adminApp
        }
      } catch (error: any) {
        alert("Error: " + error.Message)
        this.adminApp = []
        return this.adminApp
      }
    },

    async AdminUpdateStatus(appointmentId: number, payload : []) {
      try {
        const response = await axios.post(`http://localhost:5080/api/Appointment/Admin-Update-Status-Appointment/${appointmentId}`, payload, {
          headers: { 'Content-Type': 'application/json' },
          withCredentials : true
        })

        if (response.status === 200) {
          alert("Update Status Successfully")
          return true
        } else {
          alert("Update Status Failed")
          return response.status
        }
      } catch (error: any) {
        alert("Error: " + error.Message)
        return error
      }
    },

    async cancelReservation(userId: number, appId: number){
      try {
        const response = await axios.post(`http://localhost:5080/api/Appointment/UserCancelReservation/${userId}`,{ appointmentId: appId }, {
          withCredentials:true, headers:{'Content-Type' : 'application/json'}
        })

        if(response.status === 200){
          alert("Cancel Reservation Successfully")
          return true
        }else{
          alert("Cancel Reservation Failed")
          return false
        }
      } catch (error: any) {
        alert("Error Message: " + error.Message)
        return false;
      }
    },

    async AdminUpdateReservation(adminId : number, payload : []){
        try {
          const response = await axios.post(`http://localhost:5080/api/Appointment/Admin-Update-Reservation/${adminId}`, payload, {
            headers : {'Content-Type' : 'application/json'}, withCredentials : true
          })

          if(response.status === 200){
            alert("Successfully Update")
            return true;
          }else{
            alert(response.data.Message)
            return false;
          }
        } catch (error: any) {
          alert("Error: " + error.Message )
          return false;
        }
    },

    async UserUpdateReservation(userId : number, payload : []){
      try {
        const response = await axios.post(`http://localhost:5080/api/Appointment/UserUpdateReservation/${userId}`, 
          payload, {headers : {'Content-Type' : 'application/json'}, withCredentials:true}
        )

        if(response.status === 200){
          alert("You Successfully Update Your Reservation")
          return true;
        }else{
          alert("Update Reservation Failed, Theres Something wrong On Updating Reservation")
          return false;
        }
      } catch (error: any) {
        alert("Error: " + error.Message)
        return false;
      }
    }

  }
})
