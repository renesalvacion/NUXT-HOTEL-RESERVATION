import { useSessionStore } from "../stores/session";
import { navigateTo } from "#app";

export default defineNuxtRouteMiddleware(async (to, from) => {
  const sessionStore = useSessionStore();

  // Fetch session from backend
  await sessionStore.getSession();

  // Check if session exists
  if (!sessionStore.session?.email) {
    // Redirect to home/login if not logged in
    return navigateTo('/');
  }

  console.log("User session:", sessionStore.session);
});
