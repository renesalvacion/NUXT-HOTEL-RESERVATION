<template class="">
    <footer class="bg-gray-800 text-white">
        <div class="container mx-auto px-4 text-center">
            <!-- Hotel Brand Name -->
            <h3 class="text-2xl font-semibold mb-4">Hotel Ease</h3>
            
            <!-- Footer Icons: Social Media Links -->
            <div class="flex justify-center gap-8 mb-6">
                <!-- Facebook -->
                <a href="http://facebook.com" target="_blank" class="hover:text-blue-500">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="text-white">
                        <path fill="currentColor" d="M22 12c0 5.52-4.48 10-10 10S2 17.52 2 12S6.48 2 12 2s10 4.48 10 10zm-10 3h-2v-6h2v6zm-1-8.75c-.8 0-1.25-.67-1.25-1.5S10.2 4.5 11 4.5s1.25.67 1.25 1.5S11.8 6.25 11 6.25z"/>
                    </svg>
                </a>
                <!-- Instagram -->
                <a href="http://instagram.com" target="_blank" class="hover:text-purple-500">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="text-white">
                        <path fill="currentColor" d="M12 2.2c3.2 0 3.6.01 4.9.07 1.1.05 1.7.24 2.1.39.5.2.9.45 1.3.85.4.4.65.8.85 1.3.15.4.34 1 .39 2.1.06 1.3.07 1.7.07 4.9 0 3.2-.01 3.6-.07 4.9-.05 1.1-.24 1.7-.39 2.1-.2.5-.45.9-.85 1.3-.4.4-.8.65-1.3.85-.4.15-.9.34-2.1.39-1.3.06-1.7.07-4.9.07-4.9s-3.6 0-4.9-.07c-1.1-.05-1.7-.24-2.1-.39-.5-.2-.9-.45-1.3-.85-.4-.4-.65-.8-.85-1.3-.15-.4-.34-1-.39-2.1-.06-1.3-.07-1.7-.07-4.9 0-3.2.01-3.6.07-4.9.05-1.1.24-1.7.39-2.1.2-.5.45-.9.85-1.3.4-.4.8-.65 1.3-.85.4-.15.9-.34 2.1-.39 1.3-.06 1.7-.07 4.9-.07zM12 0C5.4 0 0 5.4 0 12c0 6.6 5.4 12 12 12s12-5.4 12-12c0-6.6-5.4-12-12-12z"/>
                    </svg>
                </a>
                <!-- Twitter -->
                <a href="http://twitter.com" target="_blank" class="hover:text-blue-400">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="text-white">
                        <path fill="currentColor" d="M23.64 4.59c-.87.39-1.8.66-2.78.77.99-.59 1.75-1.53 2.11-2.66-.93.56-1.95.97-3.03 1.2-.88-.93-2.13-1.51-3.53-1.51-2.68 0-4.86 2.16-4.86 4.84 0 .38.04.74.11 1.08-4.04-.2-7.61-2.14-10.01-5.07-.42.72-.66 1.55-.66 2.44 0 1.69.87 3.17 2.18 4.04-0.8-.03-1.56-.24-2.22-.59v.06c0 2.36 1.68 4.34 3.91 4.79-.41.11-.85.17-1.3.17-.32 0-.63-.03-.94-.09.63 1.97 2.45 3.41 4.61 3.45-1.68 1.32-3.81 2.1-6.12 2.1-.4 0-.79-.02-1.18-.07 2.2 1.42 4.82 2.25 7.64 2.25 9.18 0 14.21-7.62 14.21-14.21 0-.22 0-.44-.02-.66.97-.72 1.81-1.62 2.48-2.66z"/>
                    </svg>
                </a>
            </div>

            <!-- Contact Information -->
            <div class="flex justify-center gap-12 mb-6">
                <!-- Phone -->
                <div class="flex flex-col items-center">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="text-white mb-2">
                        <path fill="currentColor" d="M18.88 15.3c-.52-.1-.93-.49-1.1-.99l-1.27-3.54c-.2-.56-.68-.99-1.24-1.2-1.07-.4-3.02-1.53-4.07-3.27-1.11-1.72-.75-4.03.74-5.44l.35-.35c1.54-1.54 4.09-2.04 6.33-1.2 2.1.74 3.48 2.53 3.99 4.58.37 1.52.39 3.16.07 4.69-.25.68-.6 1.24-1.07 1.69l-.35.35c-.67.68-1.6 1.04-2.5 1.05-.11 0-.21-.01-.32-.02z"/>
                    </svg>
                    <p>(123) 456-7890</p>
                </div>
                <!-- Email -->
                <div class="flex flex-col items-center">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" class="text-white mb-2">
                        <path fill="currentColor" d="M12 2L2 7h4v6h12V7h4l-10-5zm0 6H4v4h16V8h-8z"/>
                    </svg>
                    <p>contact@hotel.com</p>
                </div>
            </div>

            <!-- Quick Links -->
            <div class="text-sm text-gray-400">
                <p>&copy; 2025 Hotel Name. All rights reserved.</p>
                <p><a href="#" class="hover:text-gray-200">Terms of Service</a> | <a href="#" class="hover:text-gray-200">Privacy Policy</a></p>
            </div>
        </div>
    </footer>
</template>

<style scoped>
    footer .hover\:text-gray-200:hover {
    color: #e2e8f0;
}

footer .hover\:text-blue-500:hover {
    color: #3b82f6;
}

footer .hover\:text-purple-500:hover {
    color: #9333ea;
}

footer .hover\:text-blue-400:hover {
    color: #60a5fa;
}
</style>
