import tailwindcss from '@tailwindcss/vite'
import google from './app/plugins/google-oauth'

export default defineNuxtConfig({
  ssr: false,
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  css: [
    '~/assets/css/main.css', // Your custom styles (optional)
  ],
  modules: [
    '@pinia/nuxt', 
    '@nuxtjs/google-fonts', 
    '@nuxt/ui',
    'nuxt-google-auth',
    
  ],

     googleAuth: {
        clientId: '635110398502-0q31qqno94m7mlpatabdm73gimpkp0tj.apps.googleusercontent.com',
        redirectUri: 'http://localhost:3000/callback', // Ensure this matches with the URI in your Google Console
        responseType: 'token id_token',
        autoLoadScript: true,         // load Google script automatically
        promptOneTap: true,           // show One Tap prompt
        enableServerVerify: true      // enable server-side token verification endpoint
    },
  
  plugins:['../frontend/app/plugins/google-oauth.js'],
  vite: {
    plugins: [
      tailwindcss(),
      
    ],
  },
})
