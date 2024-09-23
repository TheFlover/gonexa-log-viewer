<template>
	<div id="app">
		<h1>Log Viewer</h1>
		<LogTable/>
  </div>
</template>

<script>
import LogTable from './components/LogTable.vue'

export default {
	name: 'App',
	components: {
		LogTable
	}
}
</script>

<style>
</style>
