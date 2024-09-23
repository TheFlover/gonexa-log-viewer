<template>
	<div>
		<DataTable :value="logs" :paginator="true" :rows="10"
					paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
					:rowsPerPageOptions="[10,20,50]"
					currentPageReportTemplate="Showing {first} to {last} of {totalRecords}">
			<Column field="id" header="ID" sortable></Column>
			<Column field="OrgName" header="Organization" sortable></Column>
			<Column field="fileName" header="File Name" sortable></Column>
			<Column field="generationStatus" header="Status" sortable>
				<template #body="slotProps">
					<Tag :value="slotProps.data.generationStatus" :severity="getStatusSeverity(slotProps.data.generationStatus)"></Tag>
				</template>
			</Column>
			<Column field="generationStartsAt" header="Start Time" sortable>
				<template #body="slotProps">
					{{ new Date(slotProps.data.generationStartsAt).toLocaleString() }}
				</template>
			</Column>
			<Column field="generationDuration" header="Generation Duration" sortable></Column>
		</DataTable>
	</div>
</template>

<script>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';

export default {
	components: {
		DataTable,
		Column,
		Tag
	},
	setup() {
		const logs = ref([]);

		const getLogs = async () => {
			try {
				const response = await axios.get('http://localhost:5008/api/logs');
				logs.value = response.data;
			} catch (error) {
				console.error('Error fetching logs:', error);
			}
		};

		const getStatusSeverity = (status) => {
			switch(status) {
				case 'SUCCESS': return 'success';
				case 'FAILED': return 'danger';
				case 'IN_PROGRESS': return 'warning';
				default: return 'info';
			}
		};

		onMounted(getLogs);

		return {
			logs,
			getStatusSeverity
		};
	}
}
</script>
