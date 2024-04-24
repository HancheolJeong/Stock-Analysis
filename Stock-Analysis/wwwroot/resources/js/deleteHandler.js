document.addEventListener('DOMContentLoaded', function () {
	document.querySelectorAll('.delete-btn').forEach(button => {
		button.addEventListener('click', function () {
			const id = this.getAttribute('data-id');

			fetch(`/portfolio/delete/${id}`, {
				method: 'DELETE'
			})
				.then(response => response.json())
				.then(data => {
					if (data.success) {
						alert(data.message);
						location.reload();
					} else {
						alert(data.message);
					}
				})
				.catch(error => console.error('Error:', error));
		});
	});
});