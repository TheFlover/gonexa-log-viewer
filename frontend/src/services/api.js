import axios from 'axios'

const apiClient = axios.create({
    baseURL: process.env.VUE_APP_API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
})

export default {
    getLogs() {
        return apiClient.get('/api/logs')
    },
    downloadGeneratedFile(logId) {
        return apiClient.get(`/api/logs/${logId}/download/generatedfile`, {
            responseType: 'blob'
        })
    },
    downloadModelFile(logId) {
        return apiClient.get(`/api/logs/${logId}/download/modelfile`, {
            responseType: 'blob'
        })
    },
    retryGeneration(logId) {
        return apiClient.post(`/api/logs/${logId}/retry`)
    }
}