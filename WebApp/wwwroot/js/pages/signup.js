let addressLatitude = 9.958; // Default latitude
let addressLongitude = -84.121; // Default longitude
let isSelectedAddress = false; // Flag to know if the user has selected an address

// Initialize and add the map , this function is called by the google maps api
function initMap() {
    function createMap(mapOptions) {
        map = new google.maps.Map(document.getElementById('map'), mapOptions);

        marker = new google.maps.Marker({
            map: map,
            draggable: true,
            animation: google.maps.Animation.DROP,
            position: mapOptions.center
        });

        google.maps.event.addListener(marker, 'dragend', function () {
            addressLatitude = marker.getPosition().lat();
            addressLongitude = marker.getPosition().lng();
            isSelectedAddress = true;

            $('#txtAddress').val(addressLongitude + ', ' + addressLatitude);
        });
    }

    // Check if the browser supports geolocation
    if (navigator.geolocation) {

        // Get the current position of the user
        function success(position) {
            let mapOptions = {
                center: {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                },
                zoom: 13,
                streetViewControl: false
            };
            createMap(mapOptions);
        }

        // If the browser is unable to get the current location of the user
        function error() {
            let mapOptions = {
                center: {
                    lat: addressLatitude,
                    lng: addressLongitude
                },
                zoom: 9,
                streetViewControl: false
            };
            createMap(mapOptions);
        }

        navigator.geolocation.getCurrentPosition(success, error);

    } else {
        let mapOptions = {
            center: {
                lat: addressLatitude,
                lng: addressLongitude
            },
            zoom: 9,
            streetViewControl: false
        };
        createMap(mapOptions);
    }
}

// Read form data
function readFormData() {
    return new Promise((resolve) => {
        let formData = {};

        formData.firstName = $('#txtFirstName').val().trim();
        formData.lastName = $('#txtLastName').val().trim();
        formData.email = $('#txtEmail').val().trim();
        formData.identificationType = $('#cbIdentificationType option:selected').val();
        formData.identificationValue = $('#txtIdentificationValue').val().trim();
        formData.addressLatitude = isSelectedAddress ? addressLatitude : null;
        formData.addressLongitude = isSelectedAddress ? addressLongitude : null;
        formData.phoneNumbers = [];
        formData.profilePhoto = null;
        formData.themePreference = "Light";
        formData.role = "N/A";

        // get phone numbers
        $('.phone').each(function () {
            let phoneNumber = $(this).val().trim();
            if (phoneNumber) {
                formData.phoneNumbers.push(phoneNumber);
            }
        });

        // get photo file and convert to base64
        let photo = $('#profilePicture')[0].files[0];
        if (!photo) {
            resolve(formData);
            return;
        }

        let reader = new FileReader();
        reader.readAsDataURL(photo);
        reader.onload = function () {
            formData.profilePhoto = reader.result;
            resolve(formData);
        }
    });
};

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

// Validate form data
function ValidateFormData(formData) {
    hideValidationErrors();

    // Validate first name
    if (!formData.firstName || formData.firstName.length < 2) {
        showValidationErrors('#txtFirstName', 'Por favor ingrese su nombre');
        return false;
    }

    // Validate last name
    if (!formData.lastName || formData.lastName.length < 5) {
        showValidationErrors('#txtLastName', 'Por favor ingrese su apellidos');
        return false;
    }

    // Validate identification type
    if (!formData.identificationType || formData.identificationType === '') {
        showValidationErrors(null, 'Por favor seleccione un tipo de identificación');
        return false;
    }

    // Validate identification value
    if (!formData.identificationValue || formData.identificationValue.length < 6) {
        showValidationErrors('#txtIdentificationValue', 'Por favor ingrese su número de identificación');
        return false;
    }

    // Validate address
    if (!formData.addressLatitude || !formData.addressLongitude) {
        showValidationErrors(null, 'Es necesario seleccionar una dirección en el mapa');
        return false;
    }

    // Validate email
    if (!formData.email || formData.email.indexOf('@') === -1 || formData.length < 6) {
        showValidationErrors('#txtEmail', 'Por favor ingrese un correo electrónico válido');
        return false;
    }

    // Validate photo
    if (!formData.profilePhoto) {
        showValidationErrors(null, 'Por favor seleccione una foto de perfil');
        return false;
    }

    // Validate phone numbers, two are required
    if (formData.phoneNumbers.length < 2) {
        showValidationErrors(null, 'Es necesario ingresar al menos dos números de teléfono');
        return false;
    }

    // Validate phone numbers, rerquired to be 8 digits 
    for (let i = 0; i < formData.phoneNumbers.length; i++) {
        const phoneNumber = formData.phoneNumbers[i];
        // Valida is number
        if (phoneNumber.length != 8 && Number.isInteger(phoneNumber) && phoneNumber < -1) {
            showValidationErrors(null, `El numero de teléfono ${phoneNumber} debe tener 8 dígitos`);
            return false;
        }
    }

    return true;
}

// Enable or disable form controls
function EnableDisableForm(enable) {
    $('#txtFirstName').prop('disabled', !enable);
    $('#txtLastName').prop('disabled', !enable);
    $('#cbIdentificationType').prop('disabled', !enable);
    $('#txtIdentificationValue').prop('disabled', !enable);
    $('#txtEmail').prop('disabled', !enable);
    $('#profilePicture').prop('disabled', !enable);
    $('#btnAddPhone').prop('disabled', !enable);
    $('.btnRemovePhone').prop('disabled', !enable);
    $('.phone').prop('disabled', !enable);
    $('#btnCreate').prop('disabled', !enable);
}

function SignupController() {

    this.ApiService = "user";
    this.title = "Registro de Usuario";

    this.InitView = function () {
        document.title = this.title;

        $('#btnGoToLogin').click(function () {
            // go to login
            window.location.href = '/login';
        });

        $('btnGoToLandigPage').click(function () {
            // go to landing page
            window.location.href = '/landingPage';
        });

        $("#btnCreate").click(function () {
            var vc = new SignupController();
            vc.Create();
        });

        // Add phone number
        $('#btnAddPhone').click(function () {
            // add element to phone-group
            const phoneGroup = $('#phone-group');
            const phoneGroupCount = phoneGroup.children().length;
            const inputID = Date.now();

            let phone = $(`
                <div class="input-group mb-2">
                    <input id="phone-${inputID}" type="text" class="phone form-control" placeholder="Teléfono ${phoneGroupCount + 1}" autocomplete="on" />
                    <button class="btnRemovePhone btn btn-outline-secondary btn-phone" type="button">Quitar teléfono</button>
                </div>`);
            phoneGroup.append(phone);

            // Set focus on the new element
            $(`#phone-${inputID}`).focus();
        });

        // Remove phone number
        $('#phone-group').on('click', '.btnRemovePhone', function () {
            // get parent
            let parent = $(this).parent();
            parent.remove();

            // reindex phone numbers
            let count = 1;
            $('.phone').each(function () {
                // change placeholder
                $(this).attr('placeholder', `Teléfono ${count++}`);
            });
        });

        // enable #txtIdentificationValue
        $('#cbIdentificationType').change(function () {
            $('#txtIdentificationValue').val('');
            $('#txtIdentificationValue').attr("readonly", false);
        });

        $('#txtFirstName').focus();
    }

    this.Create = async function () {
        EnableDisableForm(false);
        let formData = await readFormData();
        if (!ValidateFormData(formData)) {
            EnableDisableForm(true);
            return;
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/signup";

        function successCallback(response) {
            // show success message swal
            Swal.fire({
                icon: 'success',
                title: 'Registro exitoso',
                text: 'Se ha enviado una contraseña temporal a su número de teléfono y un correo electrónico de bienvenida',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Iniciar sesión'
            }).then((result) => {
                if (result.isConfirmed) {
                    // go to login
                    window.location.href = '/login';
                }
            });
        }

        function failCallBack(response) {
            showValidationErrors(null, response);
            EnableDisableForm(true);
        }

        controlActions.PostToAPI(serviceRoute, formData, successCallback, failCallBack);
    }
}


$(document).ready(function () {
    const viewCont = new SignupController();
    viewCont.InitView();
});