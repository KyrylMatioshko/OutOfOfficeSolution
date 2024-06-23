document.addEventListener("DOMContentLoaded", function () {
    const changeRoleButtons = document.querySelectorAll('.change-role-btn');

    changeRoleButtons.forEach(button => {
        button.addEventListener('click', function () {
            const userId = this.getAttribute('data-user-id');
            const selectElement = document.querySelector(`.role-select[data-user-id='${userId}']`);
            const newRoleName = selectElement.value;

            if (newRoleName) {
                fetch(`/RoleManage/ChangeRole/${userId}/${newRoleName}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                })
                    .then(response => {
                        if (response.ok) {
                            alert('Роль успішно змінено');
                            location.reload();
                        } else {
                            alert("Не вдалося змінити роль");
                        }
                    })
                    .catch(error => console.error('Error:', error));
            } else {
                alert('Будь ласка, виберіть нову роль');
            }
        });
    });
});