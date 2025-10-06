// account.login.js
(function () {
    'use strict'

    const loginForm = document.getElementById('login-form');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const loginButtonContainer = document.getElementById('login-form-button');

    loginForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        if (!loginForm.checkValidity()) {
            loginForm.classList.add('was-validated');
            return;
        }

        const formInputs = loginForm.querySelectorAll('input');
        let model = {};
        let token = '';
        formInputs.forEach(element => {
            if (element.name === '__RequestVerificationToken') {
                token = element.value;
            } else {
                model[element.name] = element.value;
            }
        });

        usernameInput.disabled = true;
        passwordInput.disabled = true;
        loginButtonContainer.innerHTML = `
            <button class="btn btn-dark w-100 mt-4 mb-3" type="button" disabled>
                <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                Ingresando...
            </button>
        `;

        try {
            const response = await fetch('/Account/LoginEmployeeUser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(model)
            });

            const result = await response.json();

            if (result.success) {
                const location = window.location;
                window.location.href = `${location.protocol}//${location.host}${result.path}`;
            } else {
                alert(result.message);
            }
        } catch (err) {
            alert('Ha ocurrido un error en la aplicación');
        } finally {
            usernameInput.disabled = false;
            passwordInput.disabled = false;
            loginButtonContainer.innerHTML = `
                <button type="submit" class="btn btn-dark w-100 mt-4 mb-3">Ingresar</button>
            `;
        }
    });
})();