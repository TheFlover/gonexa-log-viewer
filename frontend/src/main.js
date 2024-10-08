import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura';
import ToastService from 'primevue/toastservice';

const app = createApp(App)

// Use PrimeVue
app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
});
app.use(ToastService);

document.title = process.env.VUE_APP_TITLE

app.mount('#app')
