document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.project-status-switch');
    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var projectId = button.getAttribute('data-project-id');
            var url = `/Project/SwitchStatus?projectId=${projectId}`;

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ projectId: projectId })
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        alert('Status switched successfully');
                        location.reload();
                    } else {
                        alert('Failed to switch status');
                    }
                })
                .catch(error => {
                    console.error('There has been a problem with your fetch operation:', error);
                });
        });
    });
});