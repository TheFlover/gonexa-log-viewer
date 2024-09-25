export const createErrorHandler = (toast) => {
	const handleError = async (error) => {
		let summary = 'Error';
		let message = 'An unexpected error occurred';

		if (error.response && (error.response.data instanceof Blob || typeof error.response.data === 'string')) {
			let backendMessage;
			if (error.response.data instanceof Blob) {
				backendMessage = await error.response.data.text();
			} else {
				backendMessage = error.response.data;
			}
			
			// Delete quotes if the backendMessage is a string between quotes
			message = backendMessage.replace(/^"|"$/g, '');
			summary = error.response.statusText;
		} else if (error.request) {
			summary = 'Network Error';
			message = 'The request was made but no response was received.';
		} else {
			summary = 'Error';
			message = error
		}

		toast.add({
			severity: 'error',
			summary: summary,
			detail: message,
			life: 5000
		});
	};

	return { handleError };
};