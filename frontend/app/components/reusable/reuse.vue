<template>
    <div class="p-4 border rounded shadow">
        <h2>{{ name }}</h2>
        <p>{{ email }}</p>
    </div>
</template>

<script setup>
    import { defineProps } from 'vue';

    defineProps({
        name : String,
        email : String
    })
</script>