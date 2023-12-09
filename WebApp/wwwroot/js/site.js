/// <reference path="pages/iot-pet.js" />
// Read form data
function site_readFormData() {
    let formData = {};
    formData.email = JSON.parse(sessionStorage.getItem('user')).email;
    formData.currentPassword = $('#txtCurrentPassword').val().trim();
    formData.newPassword = $('#txtNewPassword').val().trim();
    formData.confirmNewPassword = $('#txtConfirmNewPassword').val().trim();
    return formData;
};


// hide validation errors
function site_hideValidationErrors() {
    $('#validationErrorsChangePassword').addClass('d-none');
    $('#validationErrorsChangePassword').text('')
};

// show validation errors
function site_showValidationErrors(element, message) {
    $('#validationErrorsChangePassword').removeClass('d-none');
    $('#validationErrorsChangePassword').text(message);

    // Hide validation errors when the user changes the value
    if (element) {
        // Set focus on the element
        $(element).focus();
    }
};

// Validate form data
function site_validateFormData(formData) {
    site_hideValidationErrors();

    // Validate current password
    if (!formData.currentPassword) {
        site_showValidationErrors('#txtCurrentPassword', 'Debe ingresar su contraseña actual');
        return false;
    }

    // Validate new password
    if (!formData.newPassword) {
        site_showValidationErrors('#txtNewPassword', 'Debe ingresar una nueva contraseña');
        return false;
    }

    // Validate confirm new password
    if (!formData.confirmNewPassword) {
        site_showValidationErrors('#txtConfirmNewPassword', 'Debe confirmar la nueva contraseña');
        return false;
    }

    // Validate confirm new password
    if (formData.newPassword != formData.confirmNewPassword) {
        site_showValidationErrors('#txtConfirmNewPassword', 'Las contraseñas no coinciden');
        return false;
    }

    return true;
};

// Enable or disable form controls
function site_enableDisableForm(enable) {
    $('#txtCurrentPassword').prop('disabled', !enable);
    $('#txtNewPassword').prop('disabled', !enable);
    $('#txtConfirmNewPassword').prop('disabled', !enable);
    $('#btnChangePassword').prop('disabled', !enable);
};

// hide validation errors
function hideValidationErrors() {
    $('#validationErrors').addClass('d-none');
    $('#validationErrors').text('')
}

// show validation errors
function showValidationErrors(message) {
    $('#validationErrors').removeClass('d-none');
    $('#validationErrors').text(message);
}

// Title modal
function setTitleModal(title, btnText) {
    $('#titleModal').text(title);
    $('#btnSubmit').text(btnText);
}

// hide or show modal
function showModal(show) {
    if (show) {
        $('#formModal').modal('show');
    }
    else {
        $('#formModal').modal('hide');
    }
}

function formatDateTime(date) {
    let day = date.getDate();
    let month = date.getMonth() + 1;
    let year = date.getFullYear();
    let hour = date.getHours();
    let minutes = date.getMinutes();
    let ampm = hour >= 12 ? 'PM' : 'AM';

    hour = hour % 12;
    hour = hour ? hour : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;

    return `${day}/${month}/${year} ${hour}:${minutes} ${ampm}`;
}

// Using the function
let currentDate = new Date();
console.log(formatDateTime(currentDate));


function siteController() {
    this.ApiService = "user";

    this.InitView = function () {
        // event change password
        $('#btnChangePassword').click(function () {
            const viewCont = new siteController();
            viewCont.ChangePassword();
        });

        $('#btnLogout').click(function () {
            // Clear session storage
            sessionStorage.clear();
            window.location.href = '/login';
        });

        // check if user is logged in
        this.isLogged().then((response) => {
            if (response == false) {
                window.location.href = '/login';
            } else {
                const roleUser = JSON.parse(sessionStorage.getItem('user')).role;    

                if (roleUser == "client") {
                    //$('a[href="/User"]').removeClass('d-none');
                    //$('a[href="/Room"]').removeClass('d-none');
                    //$('a[href="/Service"]').removeClass('d-none');
                    //$('a[href="/Package"]').removeClass('d-none');
                    //$('a[href="/Reservations"]').removeClass('d-none');
                    $('a[href="/IoTPet"]').removeClass('d-none');
                }
            
                if (roleUser == "gestor") {
                    $('a[href="/User"]').removeClass('d-none');
                    $('a[href="/Room"]').removeClass('d-none');
                    //$('a[href="/Service"]').removeClass('d-none');
                    $('a[href="/Package"]').removeClass('d-none');
                    $('a[href="/Reservations"]').removeClass('d-none');
                    $('a[href="/IoTPet"]').removeClass('d-none');
                }

                if (roleUser == "admin") {
                    $('a[href="/User"]').removeClass('d-none');
                    $('a[href="/Room"]').removeClass('d-none');
                    $('a[href="/Service"]').removeClass('d-none');
                    $('a[href="/Package"]').removeClass('d-none');
                    $('a[href="/Reservations"]').removeClass('d-none');
                    $('a[href="/IoTPet"]').removeClass('d-none');
                }

                const viewCont = new siteController();
                viewCont.checkOtpVerification();
            }
        });
    };

    this.ChangePassword = function () {
        site_enableDisableForm(false);
        const formData = site_readFormData();

        if (!site_validateFormData(formData)) {
            site_enableDisableForm(true);
            return;
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/ChangePassword";
        const serviceRouteUser = this.ApiService + "/retrievebyid";

        function successCallback(response) {


            // Update user in session storage
            let userId = JSON.parse(sessionStorage.getItem('user')).id;
            controlActions.GetToApi(serviceRouteUser + "?id=" + userId, function (user) {
                // Store user in session storage
                sessionStorage.setItem('user', JSON.stringify(user));

                $('#modelChangePassword').modal('hide');
                site_enableDisableForm(true);
                Swal.fire({
                    title: 'Contraseña cambiada',
                    text: 'Su contraseña ha sido cambiada exitosamente',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                });
            }, failCallback);
        };

        function failCallback(response) {
            site_enableDisableForm(true);
            site_showValidationErrors(null, response);
        };

        controlActions.PutToAPI(serviceRoute, formData, successCallback, failCallback);
    };

    this.Logout = function () {
        // Clear session storage
        sessionStorage.clear();

        // Redirect to login page
        window.location.href = '/login';
    }

    this.isLogged = function () {
        const user = JSON.parse(sessionStorage.getItem('user'));
        if (user == null) {
            window.location.href = '/login';
            return;
        };

        return new Promise((resolve, reject) => {
            const controlActions = new ControlActions();
            let serviceRoute = this.ApiService + "/IsLoggedIn";

            function successCallback(response) {
                resolve(response);
            }

            function failCallBack(response) {
                resolve(false);
            }

            controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
        });
    };

    this.checkOtpVerification = function () {
        // Get id from session storage
        let user = JSON.parse(sessionStorage.getItem('user'));
        if (user == null) {
            // Redirect to login page
            window.location.href = '/login';
        }

        if (user.isPasswordRequiredChange == true) {
            $('#modelChangePassword').modal('show');
        };
    };
};

$(document).ready(async function () {
    // check if user is logged in
    viewCont = new siteController();
    viewCont.InitView();
});