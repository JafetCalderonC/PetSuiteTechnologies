// Read form data
function readFormData() {
    let formData = { id:0};
    formData.email = $('#txtEmail').val().trim();
    formData.password = $('#txtPassword').val().trim();
    return formData;
};

// Validate form data
function ValidateFormData(formData) {
    hideValidationErrors();

    // Validate email
    if (!formData.email) {
        showValidationErrors('#txtEmail', 'Debe ingresar un correo electrónico');
        return false;
    }

    // Validate password
    if (!formData.password) {
        showValidationErrors('#txtPassword', 'Debe ingresar una contraseña');
        return false;
    }

    return true;
}

// Enable or disable form controls
function EnableDisableForm(enable) {
    $('#txtEmail').prop('disabled', !enable);
    $('#txtPassword').prop('disabled', !enable);
    $('#btnLogin').prop('disabled', !enable);
    $('#btnCreateAccount').prop('disabled', !enable);
}

// hide validation errors
function hideValidationErrors() {
    $('#validationErrors').addClass('d-none');
    $('#validationErrors').text('')
}

// show validation errors
function showValidationErrors(element, message) {
    $('#validationErrors').removeClass('d-none');
    $('#validationErrors').text(message);

    // Hide validation errors when the user changes the value
    if (element) {
        // Set focus on the element
        $(element).focus();
    }
}

function LoginController() {

    this.ApiService = "user";
    this.title = "Iniciar sesión";

    this.InitView = function () {
        document.title = this.title;

        $('#btnLogin').click(function () {
            const viewCont = new LoginController();
            viewCont.Login();
        });

        $('#btnCreateAccount').click(function () {
            // go to signup page
            window.location.href = '/signup';
        });

        $('#btnForgotPassword').click(function () {
            // go to forgot password page
            window.location.href = '/forgotpassword';
        });

        $('#txtEmail').focus();
    }

    this.Login = function () {
        EnableDisableForm(false);
        const formData = readFormData();

        if (!ValidateFormData(formData)) {
            EnableDisableForm(true);
            return;
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/login";
        const serviceRouteUser = this.ApiService + "/retrievebyid";

        function successCallback(response) {
            // Store token in session storage
            sessionStorage.setItem('token', response.token);
            
            // get user info from API
            controlActions.GetToApi(serviceRouteUser + "?id=" + response.userId, function (user) {
                // Store user in session storage
                sessionStorage.setItem('user', JSON.stringify(user));

                // Redirect to home page
                window.location.href = '/';
            });
        }
        function failCallBack(response) {
            showValidationErrors(null, response);
            EnableDisableForm(true);
        }

        controlActions.PostToAPI(serviceRoute, formData, successCallback, failCallBack);
    };
};

$(document).ready(function () {
    const viewCont = new LoginController();
    viewCont.InitView();
});