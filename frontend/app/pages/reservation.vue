<!--reservation.vue-->

<template>
  <Navbar />
  <div class="p-6" v-if="session?.role !== 'Admin'">
    <h1 class="text-2xl font-bold mb-4">My Reservations</h1>

    <div v-if="appointmentStore.Appointments.length" class="overflow-x-auto">
      <table
        class="min-w-full bg-white border border-gray-200 rounded-md shadow-sm"
      >
        <thead class="bg-gray-100">
          <tr>
            <th class="py-3 px-6 border-b text-left">#</th>
            <th class="py-3 px-6 border-b text-left">Date</th>
            <th class="py-3 px-6 border-b text-left">Room</th>
            <th class="py-3 px-6 border-b text-left">Number of Bed</th>
            <th class="py-3 px-6 border-b text-left">Number of Head</th>
            <th class="py-3 px-6 border-b text-left">Price</th>
            <th class="py-3 px-6 border-b text-left">Status</th>
            <th class="py-3 px-6 border-b text-left">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(app, index) in appointmentStore.Appointments"
            :key="app.appointmentId"
            class="hover:bg-gray-50 font-semibold"
          >
            <td class="py-2 px-6 border-b">{{ index + 1 }}</td>
            <td class="py-2 px-6 border-b">
              {{ new Date(app.created).toLocaleString() }}
            </td>
            <td class="py-2 px-6 border-b">{{ app.roomType }}</td>
            <td class="py-2 px-6 border-b">{{ app.numberBed }}</td>
            <td class="py-2 px-6 border-b">{{ app.headPerson }}</td>
            <td class="py-2 px-6 border-b">{{ app.price }}</td>
            <td class="py-2 px-6 border-b">{{ app.status }}</td>
            <td class="py-2 px-6 border-b flex gap-2">
              <button @click="isUserEditModal(app.appointmentId)"
                class="cursor-pointer bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
              >
                Edit
              </button>
              <button @click="isUserReviewModal"
                class="cursor-pointer bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
              >
                Review
              </button>
              <button @click="isUserCancelModal(app.appointmentId)"
                class="cursor-pointer bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
              >
                Cancel
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-else class="text-gray-500 mt-4">No reservations found.</div>
  </div>

  <div class="admin_table mt-6 pt-10" v-else>
    <h1 class="text-2xl font-bold p-3 text-center">Admin Table</h1>

    <!-- Card Container for the Table -->
    <div class="max-w-7xl mx-auto p-4 bg-white rounded-lg shadow-md">
      <!-- Table with overflow handling -->
      <div
        v-if="appointmentStore.AdminViewAppointment?.length > 0"
        class="overflow-x-auto"
      >
        <table
          class="min-w-full bg-white border border-gray-200 rounded-md shadow-sm table-auto"
        >
          <thead class="bg-gray-100">
            <tr>
              <th class="py-2 px-4 border-b text-left">#</th>

              <th class="py-2 px-4 border-b text-left">Name</th>
              <th class="py-2 px-4 border-b text-left">Room</th>
              <th class="py-2 px-4 border-b text-left">Number of Bed</th>
              <th class="py-2 px-4 border-b text-left">Number of Head</th>
              <th class="py-2 px-4 border-b text-left">Price</th>
             
              <th class="py-2 px-4 border-b text-left">No. Reservation</th>
              <th class="text-center py-2 px-4 border-b">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(app, index) in appointmentStore.AdminViewAppointment"
              :key="app.appointmentId"
              class="cursor-pointer hover:bg-gray-50 font-semibold"
            >
              <td class="py-2 px-4 border-b">{{ index + 1 }}</td>
              <td class="py-2 px-4 border-b">{{ app.crudsName }}</td>
              <td class="py-2 px-4 border-b">{{ app.roomType }}</td>
              <td class="py-2 px-4 border-b">{{ app.numberBed }}</td>
              <td class="py-2 px-4 border-b">{{ app.appointmentId }}</td>
              <td class="py-2 px-4 border-b">{{ app.price }}</td>
          
              <td class="py-2 px-4 border-b">{{ app.reservationCount }}</td>
              <td class="py-2 px-4 border-b">
                <div class="btn flex gap-3">
                  <button
                    @click="ViewAppointment(app.crudId)"
                    class="cursor-pointer bg-blue-400 text-white px-4 py-2 rounded hover:bg-blue-500 transition"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M12 16q1.875 0 3.188-1.312T16.5 11.5t-1.312-3.187T12 7T8.813 8.313T7.5 11.5t1.313 3.188T12 16m0-1.8q-1.125 0-1.912-.788T9.3 11.5t.788-1.912T12 8.8t1.913.788t.787 1.912t-.787 1.913T12 14.2m0 4.8q-3.65 0-6.65-2.037T1 11.5q1.35-3.425 4.35-5.462T12 4t6.65 2.038T23 11.5q-1.35 3.425-4.35 5.463T12 19m0-2q2.825 0 5.188-1.487T20.8 11.5q-1.25-2.525-3.613-4.012T12 6T6.813 7.488T3.2 11.5q1.25 2.525 3.613 4.013T12 17"
                      />
                    </svg>
                  </button>
                  <button
                    @click="openModal(app.appointmentId)"
                    class="cursor-pointer bg-fuchsia-500 text-white px-4 py-2 rounded hover:bg-fuchsia-600 transition"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M5 19h1.425L16.2 9.225L14.775 7.8L5 17.575zm-2 2v-4.25L16.2 3.575q.3-.275.663-.425t.762-.15t.775.15t.65.45L20.425 5q.3.275.438.65T21 6.4q0 .4-.137.763t-.438.662L7.25 21zM19 6.4L17.6 5zm-3.525 2.125l-.7-.725L16.2 9.225z"
                      />
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- No appointments message -->
      <div v-else class="text-center text-gray-500 mt-4">
        No reservations found.
      </div>
    </div>

    <!-- Pagination -->
    <div
      v-if="appointmentStore.totalPages > 1"
      class="mt-4 flex justify-center gap-2 items-center"
    >
      <!-- Prev button -->
      <button
        @click="prevPage"
        :disabled="appointmentStore.pageNumber === 1"
        class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
      >
        Prev
      </button>

      <!-- Page number buttons -->
      <button
        v-for="page in appointmentStore.totalPages"
        :key="page"
        @click="goToPage(page)"
        :class="[
          'cursor-pointer px-4 py-2 rounded hover:bg-gray-400',
          appointmentStore.pageNumber === page
            ? 'bg-blue-600 text-white'
            : 'bg-gray-300',
        ]"
      >
        {{ page }}
      </button>

      <!-- Next button -->
      <button
        @click="nextPage"
        :disabled="appointmentStore.pageNumber === appointmentStore.totalPages"
        class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
      >
        Next
      </button>
    </div>
  </div>

  <admin_edit_modal v-if="isOpen" @close="isOpen = false" :appointment-id="appId"/>

  <!--User Modal-->
  <user_cancel_modal v-if="userCancelModal" @close="userCancelModal = false" :appointment-id="appId"/>
  <user_edit_modal v-if="userEditModal" @close="userEditModal = false" :appointment-id="appId"/>
  <user_review_modal v-if="userReviewModal" @close="userReviewModal = false" />
</template>

<script setup lang="ts">
import Navbar from "~/components/navbar.vue";
import { SetAppointment } from "~/stores/appointment";
import { useSessionStore } from "#imports";
import { onMounted, ref } from "vue";
import admin_edit_modal from "~/components/admin_edit_modal.vue";
import { useRouter } from "vue-router";

import user_cancel_modal from "~/components/user/user_cancel_modal.vue";
import user_edit_modal from "~/components/user/user_edit_modal.vue";
import user_review_modal from "~/components/user/user_review_modal.vue";



interface Session {
  id: number;
  role?: string | null;
  name?: string;
}

const sessionStore = useSessionStore();
const appointmentStore = SetAppointment();

const session = ref<Session | null>(null);

const router = useRouter();
const appId = ref(0)
//modal
const isOpen = ref(false);
//user modal
const userEditModal = ref(false)
const userCancelModal = ref(false)
const userReviewModal = ref(false)

//modal function
const openModal = (appointmentId : number) => {
  appId.value = appointmentId
  isOpen.value = true;
};

//user modal function
const isUserEditModal = (appointmentId: number) => {
  appId.value = appointmentId
  userEditModal.value = true
}
const isUserCancelModal = (appointmentId : number) => {
  userCancelModal.value = true
  appId.value = appointmentId
  console.log("Appointment ID: " + appId.value);
  
}
const isUserReviewModal = () => {
  userReviewModal.value = true
}

const ViewAppointment = (userId: number) => {
  router.push(`/admin/view-details/${userId}`);
};

const nextPage = () => {
  if (appointmentStore.pageNumber < appointmentStore.totalPages) {
    appointmentStore.ViewAdminCatalog(appointmentStore.pageNumber + 1);
  }
};

const prevPage = () => {
  if (appointmentStore.pageNumber > 1) {
    appointmentStore.ViewAdminCatalog(appointmentStore.pageNumber - 1);
  }
};

// Method to go to a specific page
const goToPage = (page: number) => {
  if (page >= 1 && page <= appointmentStore.totalPages) {
    appointmentStore.ViewAdminCatalog(page, appointmentStore.pageSize);
  }
};

onMounted(async () => {
  const s = await sessionStore.getSession();
  console.log("Session Loaded:", s);

  // Map userId to id if your interface expects it
  session.value = {
    email: s?.email,
    id: s?.userId,
    role: s?.role
  };

  if (session.value?.id) {
    appointmentStore.getAppointment(session.value.id);
  }

  if (session.value?.role === "Admin") {
    await appointmentStore.ViewAdminCatalog(1, 10);
  }
});

</script>
