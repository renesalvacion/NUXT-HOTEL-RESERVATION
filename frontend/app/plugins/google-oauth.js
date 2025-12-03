// plugins/google-oauth.js
/*
import { defineNuxtPlugin } from '#app'
import GAuth from 'vue-google-oauth2'

export default defineNuxtPlugin(nuxtApp => {
  // Check if nuxtApp and nuxtApp.vueApp are available
  if (nuxtApp.vueApp && typeof nuxtApp.vueApp.use === 'function') {
    const gauthOption = {
      clientId: '635110398502-0q31qqno94m7mlpatabdm73gimpkp0tj.apps.googleusercontent.com',
      redirectUri: 'http://localhost:3000/callback',  // Adjust the callback URL
      responseType: 'token id_token',
      autoLoadScript: true,         // load Google script automatically
      promptOneTap: true,           // show One Tap prompt
      enableServerVerify: true      // enable server-side token verification endpoint
    }

    // Initialize the Google Auth plugin
    nuxtApp.vueApp.use(GAuth, gauthOption)
  } else {
    console.error('Error: nuxtApp.vueApp is not available or is not an object.')
  }
})*/
