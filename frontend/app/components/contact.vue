<template>
  <section 
    class="pt-16 flex flex-col items-center p-12 px-4 relative bg-cover bg-center bg-no-repeat"
    style="background-image: url('/images/contact.avif');"
  >
    <!-- 🔥 Overlay -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Content above overlay -->
    <div class="relative z-10 w-full flex flex-col items-center p-10">

      <h5 class="text-center font-bold text-3xl mb-8 tracking-wide text-white">
        CONTACT US
      </h5>

      <div class="w-full max-w-lg bg-white/90 backdrop-blur-md shadow-xl rounded-2xl p-8 card_form">
      <form @submit.prevent="sendEmail" ref="contactForm" class="space-y-5">
  <!-- Subject -->
  <div class="flex flex-col">
    <label class="font-semibold text-gray-700 mb-1">Subject</label>
    <input 
      type="text" 
      v-model="form.subject"
      name="subject"   
      placeholder="Enter subject..." 
      class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none"
    >
  </div>

  <!-- Email -->
  <div class="flex flex-col">
    <label class="font-semibold text-gray-700 mb-1">Email</label>
    <input 
      type="email" 
      v-model="form.email"
      name="email"   
      placeholder="Enter your email..." 
      class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none"
    >
  </div>

  <!-- Name -->
  <div class="flex flex-col">
    <label class="font-semibold text-gray-700 mb-1">Name</label>
    <input 
      type="text" 
      v-model="form.name"
      name="name"   
      placeholder="Enter your name..." 
      class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none"
    >
  </div>

  <!-- Message -->
  <div class="flex flex-col">
    <label class="font-semibold text-gray-700 mb-1">Message</label>
    <textarea 
      rows="4"
      v-model="form.message"
      name="message"  
      placeholder="Enter your message..."
      class="border rounded-lg p-3 resize-none focus:ring-2 focus:ring-blue-500 outline-none"
    ></textarea>
  </div>

  <!-- Submit Button -->
  <button 
    type="submit" 
    class="w-full bg-blue-600 text-white py-3 rounded-xl font-semibold hover:bg-blue-700 transition">
    Send Message
  </button>
</form>

      </div>

    </div>
  </section>
</template>

<script lang="ts">
import emailjs from '@emailjs/browser';

export default {
  data() {
    return {
      form: {
        subject: '',
        name: '',
        email: '',
        message: ''
      }
    };
  },
  methods: {
    sendEmail() {
      const { subject, name, email, message } = this.form;

      // Replace with your actual EmailJS credentials
      const serviceId = 'service_qyg0l6l';
      const templateId = 'template_0aqo1rf';  // Use your template ID here
      const userId = 'rlboW4GzRw5Glalhl';  // Replace with your EmailJS User ID

      // Access the form via Vue ref
      const form = this.$refs.contactForm as HTMLFormElement;

      // Initialize EmailJS and send the form
      emailjs
        .sendForm(serviceId, templateId, form, userId)  // Pass the form DOM element here
        .then(
          (response) => {
            alert('Message sent successfully!');
            console.log(response);
          },
          (error) => {
            alert('Failed to send message');
            console.error(error);
          }
        );
    }
  }
};
</script>