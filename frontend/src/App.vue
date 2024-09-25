<template>
	<div id="app">
		<Toast />
		<h1>{{ appTitle }}</h1>
		<LogTable/>
  </div>
</template>

<script>
import { ref } from 'vue';
import LogTable from './components/LogTable.vue'
import Toast from 'primevue/toast';

export default {
	name: 'App',
	components: {
		LogTable,
		Toast
	},
	setup() {
		const appTitle = ref(process.env.VUE_APP_TITLE);

		return {
			appTitle
		};
	}
}
</script>
