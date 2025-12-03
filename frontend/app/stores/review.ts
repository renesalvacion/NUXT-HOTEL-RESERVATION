import { defineStore } from "pinia";
import axios from "axios";
import { tr } from "@nuxt/ui/runtime/locale/index.js";



export const useReviewStore = defineStore("review", {
    state : () => ({
        Review : [] as any [],
        AdminReview : [] as any [],
        ViewSpecificReviewArray: [] as any []
    }),

    actions: {
        async PostReview(userId : number, payload : []){
            try {
                const response = await axios.post(`http://localhost:5080/review/Review/${userId}`, payload, {
                    headers : {'Content-Type' : 'application/json'},
                    withCredentials : true
                })

                if(response.status === 200){
                    console.log(response);
                    alert("Successfully Added Review")
                }else{
                    alert("There's something wrong")
                }
            } catch (error:any) {
                alert("Error: " + error.Message)
            }
        },

        async ViewReviewRoom(roomId: number){
            try {
                const response = await axios.get(`http://localhost:5080/review/Review/${roomId}`, {
                    withCredentials : true
                })

                if(response.status === 200){
                    this.Review = response.data
                    
                }else{
                    alert("Theres something wrong on the data.")
                }
            } catch (error: any) {
                alert("Error: " + error.message)
            }
        },

        async AdminViewReview(){
            try {
                const token = localStorage.getItem("token");
                const response = await axios.get("http://localhost:5080/review/Review/Admin-View-Review",{
                    headers:{Authorization: `Bearer ${token}`}
                })

                if(response.status === 200){
                    //const data = JSON.stringify(response.data, null, 2));
                    this.AdminReview = response.data.results.results


                    return this.AdminReview
                }else return this.AdminReview = []
            } catch (error: any) {
                alert("There's something wrong on the fetching")
                console.log("Error: " + error.Message);
                return this.AdminReview = []
            }
        },

async ViewSpecificReview(roomId: number) {
    try {
        const response = await axios.get(
            `http://localhost:5080/review/Review/View-Specific-Review/${roomId}`, 
            { withCredentials: true }
        )

        if (response.status === 200) {
            // Access the nested array correctly
            this.ViewSpecificReviewArray = response.data.result?.result || []
            console.log("Review Result:", this.ViewSpecificReviewArray)
        } else {
            this.ViewSpecificReviewArray = []
        }
    } catch (error: any) {
        alert("Error Message: " + error.message)
        this.ViewSpecificReviewArray = []
    }
}

    }
})