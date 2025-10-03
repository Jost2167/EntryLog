(function () {

    'use strict'

    const registerForm = document.getElementById('register-form');

    registerForm.addEventListener('submit', function (event) {
        event.preventDefault();

        if (!registerForm.checkValidity()) {
            registerForm.classList.add('was-validated');
        } else {
            const formInputs = registerForm.querySelectorAll('input');

            let model = {};

            formInputs.forEach((element, index) => {
                model[element.name] = element.value;
            });

            $.ajax({
                url: '/Account/RegisterEmployeeUser',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(model), // 👈 ya no lo metas en { model }
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                beforeSend: () => {
                    $('#documentNumber').attr('disabled', true);
                    $('#username').attr('disabled', true);
                    $('#cellPhone').attr('disabled', true);
                    $('#password').attr('disabled', true);
                    $('#passwordConf').attr('disabled', true);
                    $('#register-form-button').html(`
                        <button class="btn btn-dark w-100 mt-4 mb-3" type="button" disabled>
                            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                            Registrando...
                        </button>
                    `);
                },
                complete: () => {
                    $('#documentNumber').removeAttr('disabled');
                    $('#username').removeAttr('disabled');
                    $('#cellPhone').removeAttr('disabled');
                    $('#password').removeAttr('disabled');
                    $('#passwordConf').removeAttr('disabled');
                    $('#register-form-button').html(`
                        <button type="submit" class="btn btn-dark w-100 mt-4 mb-3">Registrarse</button>
                    `);
                },
                success: (result) => {
                    if (result.success) {
                        const location = window.location;
                        const url = `${location.protocol}//${location.host}${result.path}`;
                        window.location.href = url;
                    } else {
                        $.notify({
                            // options
                            icon: 'icon-bell',
                            title: 'Notificación',
                            message: result.message,
                        }, {
                            // settings
                            type: "warning",
                        });
                    }
                },
                error: (err) => {
                    $.notify({
                        // options
                        icon: 'icon-bell',
                        title: 'Notificación',
                        message: 'Ha ocurrido un error en la aplicación',
                    }, {
                        // settings
                        type: "danger",
                    });
                }
            })
        }
    });
})();