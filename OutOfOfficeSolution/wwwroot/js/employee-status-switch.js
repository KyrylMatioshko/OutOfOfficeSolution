document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.switch-status');
    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();

            var employeeId = button.getAttribute('data-employee-id');
            var url = `/Employee/SwitchStatus?employeeId=${employeeId}`;

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ employeeId: employeeId })
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