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
			<Column header="Download Documents">
				<template #body="slotProps">
					<div class="action-buttons">
						<Button label="Model" @click="downloadFile(slotProps.data.id, FileType.Model)" class="mr-2"/>
						<Button label="Generated" @click="downloadFile(slotProps.data.id, FileType.Generated)" />
					</div>
				</template>
			</Column>
			<Column header="Actions">
				<template #body="slotProps">
					<Button v-if="slotProps.data.generationStatus === GenerationStatus.FAILED" label="Retry generation" @click="retryGeneration(slotProps.data.id)" />
				</template>
			</Column>
		</DataTable>
	</div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { useToast } from 'primevue/usetoast';
import { createErrorHandler } from '@/services/errorHandler';
import apiService from '@/services/api';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';
import Button from 'primevue/button';

const FileType = {
  Model: 'MODEL',
  Generated: 'GENERATED'
};

const GenerationStatus = {
  SUCCESS: 'SUCCESS',
  FAILED: 'FAILED',
  IN_PROGRESS: 'IN_PROGRESS'
};

export default {
	components: {
		DataTable,
		Column,
		Tag,
		Button
	},
	setup() {
		const toast = useToast();
		const { handleError } = createErrorHandler(toast);
		const logs = ref([]);

		const getLogs = async () => {
			try {
				const response = await apiService.getLogs();
				logs.value = response.data;
			} catch (error) {
				handleError(error);
			}
		};

		const getStatusSeverity = (status) => {
			switch(status) {
				case GenerationStatus.SUCCESS: return 'success';
				case GenerationStatus.FAILED: return 'danger';
				case GenerationStatus.IN_PROGRESS: return 'warning';
				default: return 'info';
			}
		};

		const downloadFile = async (logId, fileType) => {
			try {
				let response;

				switch(fileType) {
					case FileType.Model:
						response = await apiService.downloadModelFile(logId);
						break;
					case FileType.Generated:
						response = await apiService.downloadGeneratedFile(logId);
						break;
					default:
						handleError('Invalid file type: ' + fileType);
						return;
				}

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

				toast.add({
					severity: 'success',
					summary: 'Downloaded',
					detail: 'File downloaded successfully',
					life: 5000
				})
			} catch (error) {
				handleError(error);
			}
		};

		// Retry generation for a log
		const retryGeneration = async (logId) => {
			try {
				const response = await apiService.retryGeneration(logId);
				
				// Update the generation status of the log
				const logIndex = logs.value.findIndex(log => log.id === logId);
				if (logIndex !== -1) {
					logs.value[logIndex] = response.data;
				}

				toast.add({
					severity: 'success',
					summary: 'Retry Generation',
					detail: 'Generation has been retry successfully',
					life: 5000
				});
			} catch (error) {
				handleError(error);
			}
		};

		onMounted(getLogs);

		return {
			logs,
			getStatusSeverity,
			downloadFile,
			retryGeneration,
			FileType,
			GenerationStatus  
		};
	}
}
</script>

<style scoped>
.action-buttons {
	display: flex;
	gap: 0.5rem;
}
</style>