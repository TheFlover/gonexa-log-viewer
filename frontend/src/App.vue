<template>
	<div id="app">
		<h1>{{ appTitle }}</h1>
		<LogTable/>
  </div>
</template>

<script>
import { ref } from 'vue';
import LogTable from './components/LogTable.vue'

export default {
	name: 'App',
	components: {
		LogTable
	},
	setup() {
		const appTitle = ref(process.env.VUE_APP_TITLE);

		return {
			appTitle
		};
	}
}
</script>
