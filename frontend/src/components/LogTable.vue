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
			<Column header="Actions">
				<template #body="slotProps">
					<Button label="Download" icon="pi pi-download" @click="downloadFile(slotProps.data.id)" />
				</template>
			</Column>
		</DataTable>
	</div>
</template>

<script>
import { ref, onMounted } from 'vue';
import apiService from '@/services/api';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';
import Button from 'primevue/button';

export default {
	components: {
		DataTable,
		Column,
		Tag,
		Button
	},
	setup() {
		const logs = ref([]);

		const getLogs = async () => {
			try {
				const response = await apiService.getLogs();
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

		const downloadFile = async (logId) => {
			try {
				const response = await apiService.downloadFile(logId);

				// Extract file name from response headers
				const fileName = response.headers['content-disposition'].split('filename=')[1].split(';')[0];

				// Create a blob from the response data
				const blob = new Blob([response.data], { type: response.headers['content-type'] })

				// Create a URL for the blob and create a link to download the file
				const url = window.URL.createObjectURL(blob);
				const link = document.createElement('a');
				link.href = url;
				link.setAttribute('download', fileName);
				document.body.appendChild(link);
				link.click();
				link.remove();
				window.URL.revokeObjectURL(url);
			} catch (error) {
				console.error('Error downloading file:', error);
			}
		};

		onMounted(getLogs);

		return {
			logs,
			getStatusSeverity,
			downloadFile
		};
	}
}
</script>
