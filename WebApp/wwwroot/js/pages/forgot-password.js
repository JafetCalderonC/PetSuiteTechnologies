function ForgotPasswordController() {
    this.ApiService = "user";
    this.title = "Restablecer contraseña";

    this.InitView = function () {
        document.title = this.title;

        $("#btnForgotPassword").click(function () {
            const viewCont = new ForgotPasswordController();
            viewCont.ForgotPassword();
        });

        $("#btnGoToLogin").click(function () {
            window.location.href = "/login";
        });

    };

    this.ForgotPassword = function () {
        $('#txtAlert').addClass('d-none');
        $('#txtAlert').removeClass('alert-danger');
        $('#txtAlert').removeClass('alert-success');

        let email = $("#txtEmail").val();

        if (email == "") {
            $('#txtAlert').text("Por favor ingrese su correo electrónico");
            $('#txtAlert').removeClass('d-none');
            $('#txtAlert').addClass('alert-danger');
            return;
        }

        let data = {
            Email: email
        };


        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/forgotpassword";

        function successCallback(response) {
            $('#txtAlert').text("Se ha enviado un correo electrónico para restablecer su contraseña");
            $('#txtAlert').removeClass('d-none');
            $('#txtAlert').addClass('alert-success');
        }

        function failCallback(response) {
            $('#txtAlert').text(response);
            $('#txtAlert').removeClass('d-none');
            $('#txtAlert').addClass('alert-danger');
        }

        controlActions.PutToAPI(serviceRoute, data, successCallback, failCallback);
    };
}


$(document).ready(function () {
    const viewCont = new ForgotPasswordController();
    viewCont.InitView();
});