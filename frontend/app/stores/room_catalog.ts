import { defineStore } from "pinia";
import axios from "axios";
import { error } from "#build/ui";

export const useRoomCatalogStore = defineStore("catalog", {
    state: () => ({
        catalog: {
        rooms: [] as any[],  // List of rooms
        totalCount: 0, // Total number of rooms for pagination
        },
        details: null as any | null,  // Details of a single room, initially null
        error: '', // Error message
        loading: false, // Loading state, useful for UI feedback
    }),


    actions : {
        async CreateCatalog(payload : any) {
            
            try {
                const token = localStorage.getItem("token"); // should match what backend expects
                const response = await axios.post("http://localhost:5080/admin/api/RoomCatalog/room-catalog", payload, {
                    headers:{Authorization: `Bearer ${token}`,'Content-Type': 'multipart/form-data'}, withCredentials : true
                })

                if(response.status === 200){
                    alert("Room Catalog Added Successfully")
                    return true;
                }else{
                    alert("Room Catalog Added Failed")
                    return false;
                }
            } catch (error: any) {
                if (error.response && error.response.data) {
                    // Check if backend sent a "message"
                    const msg = error.response.data.message || JSON.stringify(error.response.data)
                    alert(msg)
                } else {
                    alert(error.message)
                }
                return this.error = error.message
            }
        },

        async GetCatalog(pageNumber = 1, pageSize = 10) {
            try {
                const response = await axios.get(`http://localhost:5080/admin/api/RoomCatalog`, {
                    params: { pageNumber, pageSize },
                });

                console.log("API Response:", response.data.rooms);
                console.log("API averageStars:", response.data.rooms.averageStars);

                if (response.status === 200) {
                    // Assign the array directly
                    this.catalog.rooms = response.data.rooms || [];
                    this.catalog.totalCount = response.data.totalCount || 0;
                } else {
                    alert("Error fetching room data.");
                }
            } catch (error: any) {
                alert("There was an error fetching the data: " + error.message);
            }
        },
        async GetDetailsRoom(roomId: number){
            try {
                const response = await axios.get(`http://localhost:5080/admin/api/RoomCatalog/${roomId}`, {withCredentials:true})
                if(response.status === 200){
                    this.details = response.data.room[0] || null
                    console.log(response.data.room);
                    return this.details
                }else{
                    this.details = null
                    alert("There's something wrong on the fething")
                    return this.details
                }
            } catch (error: any) {
                this.details = null
                alert(error.Message)
                
            }
        }
    }
})