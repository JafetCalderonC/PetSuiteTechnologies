let addressLatitude = 9.958;    // Default latitude
let addressLongitude = -84.121; // Default longitude
let isSelectedAddress = false;  // Flag to know if the user has selected an address
let map;                        // Google map

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

        // get hidden fields only update
        formData.id = $('#txtHideId').val();
        formData.hideProfilePhoto = $('#textHideProfilePhoto').val();

        // get form data
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
        formData.role = $('#cbRole option:selected').val();
        formData.status = $('#cbStatus option:selected').val()

        // get phone numbers
        $('.phone').each(function () {
            let phoneNumber = $(this).val().trim();
            if (phoneNumber) {
                formData.phoneNumbers.push(phoneNumber);
            }
        });


        // get photo file and convert to base64
        let photo = $('#profilePicture')[0].files[0];

        // if id is null, then is a new user
        if (formData.id == null || formData.id == "") {

            // if photo is null, no photo was selected
            if (!photo) {
                // return without photo
                resolve(formData);
            }

            // convert photo to base64
            let reader = new FileReader();
            reader.readAsDataURL(photo);
            reader.onload = function () {
                // return with photo
                formData.profilePhoto = reader.result;
                resolve(formData);
            }
        }
        else {
            // if photo is null, not update photo
            if (!photo) {
                // return with photo from database
                formData.profilePhoto = formData.hideProfilePhoto;
                resolve(formData);
                return;
            }

            // convert photo to base64
            let reader = new FileReader();
            reader.readAsDataURL(photo);
            reader.onload = function () {
                // return with photo
                formData.profilePhoto = reader.result;
                resolve(formData);
            }
        }
    });
};

// set form data
function setFormData(data) {
    $('#txtHideId').val(data.id);
    $('#txtFirstName').val(data.firstName);
    $('#txtLastName').val(data.lastName);
    $('#txtEmail').val(data.email);
    $('#cbIdentificationType').val(data.identificationType);
    $('#txtIdentificationValue').val(data.identificationValue);
    $('#txtIdentificationValue').attr('readonly', false);
    $('#txtAddress').val(data.addressLongitude + ', ' + data.addressLatitude);
    $('#cbRole').val(data.role);
    $('#textHideProfilePhoto').val(data.profilePhoto);
    $('#imgProfilePhoto').attr('src', data.profilePhoto);
    $('#cbStatus').val(data.status);

    // ser address
    addressLatitude = data.addressLatitude;
    addressLongitude = data.addressLongitude;
    isSelectedAddress = true;
    // update map   
    initMap();

    // add phone numbers
    for (let i = 0; i < data.phoneNumbers.length; i++) {
        const phoneNumber = data.phoneNumbers[i].replace("+506", "");
        // add element to phone-group
        const phoneGroup = $('#phone-group');
        const phoneGroupCount = phoneGroup.children().length;
        const inputID = Date.now();

        let phone = $(`
                <div class="input-group mb-2">
                    <input id="phone-${inputID}" type="text" class="phone form-control" placeholder="Teléfono ${phoneGroupCount + 1}" autocomplete="on" value="${phoneNumber}" />
                    <button class="btnRemovePhone btn btn-outline-secondary btn-phone" type="button">Quitar teléfono</button>
                </div>`);
        phoneGroup.append(phone);
    }
}

// reset form data
function resetFormData() {
    $('#txtHideId').val("");
    $('#txtFirstName').val("");
    $('#txtLastName').val("");
    $('#txtEmail').val("");
    $('#cbIdentificationType').val("");
    $('#txtIdentificationValue').val("");
    $('#cbIdentificationValue').attr("readonly", false);
    $('#cbStatus').val('');
    $('#txtAddress').val("");
    $('#cbRole').val("");
    $('#textHideProfilePhoto').val("");

    // Default map
    addressLatitude = 9.958;
    addressLongitude = -84.121;
    isSelectedAddress = false;
    map.setCenter({
        lat: addressLatitude,
        lng: addressLongitude
    });
    map.setZoom(9);
    marker.setPosition({
        lat: addressLatitude,
        lng: addressLongitude
    });

    // remove phone numbers
    $('#phone-group').empty();

    // remove photo
    $('#imgProfilePhoto').attr('src', '');
    $('#profilePicture').val(null);

    // enable form
    enableDisableForm(true);

    // hide validation errors
    hideValidationErrors();
}

// Validate form data
function validateFormData(formData) {
    hideValidationErrors();

    // Validate first name
    if (!formData.firstName || formData.firstName.length < 2) {
        showValidationErrors('Por favor ingrese su nombre');
        return false;
    }

    // Validate last name
    if (!formData.lastName || formData.lastName.length < 5) {
        showValidationErrors('Por favor ingrese su apellidos');
        return false;
    }

    // Validate role
    if (!formData.role || formData.role === '') {
        showValidationErrors('Por favor seleccione un rol');
        return false;
    }

    // Validate identification type
    if (!formData.identificationType || formData.identificationType === '') {
        showValidationErrors('Por favor seleccione un tipo de identificación');
        return false;
    }

    // Validate identification value
    if (!formData.identificationValue || formData.identificationValue.length < 6) {
        showValidationErrors('Por favor ingrese su número de identificación');
        return false;
    }

    // Validate address
    if (!formData.addressLatitude || !formData.addressLongitude) {
        showValidationErrors('Es necesario seleccionar una dirección en el mapa');
        return false;
    }

    // Validate status
    if (!formData.status || formData.status === '') {
        showValidationErrors('Por favor seleccione un estado');
        return false;
    }

    // Validate email
    if (!formData.email || formData.email.indexOf('@') === -1 || formData.length < 6) {
        showValidationErrors('Por favor ingrese un correo electrónico válido');
        return false;
    }

    // Validate photo
    if (!formData.profilePhoto) {
        showValidationErrors('Por favor seleccione una foto de perfil');
        return false;
    }

    // Validate phone numbers, two are required
    if (formData.phoneNumbers.length < 2) {
        showValidationErrors('Es necesario ingresar al menos dos números de teléfono');
        return false;
    }

    // Validate phone numbers, rerquired to be 8 digits 
    for (let i = 0; i < formData.phoneNumbers.length; i++) {
        const phoneNumber = formData.phoneNumbers[i];
        // Valida is number
        if (phoneNumber.length != 8 && Number.isInteger(phoneNumber) && phoneNumber < -1) {
            showValidationErrors(`El numero de teléfono ${phoneNumber} debe tener 8 dígitos`);
            return false;
        }
    }

    return true;
}

// Enable or disable form controls
function enableDisableForm(enable) {
    $('#txtFirstName').prop('disabled', !enable);
    $('#txtLastName').prop('disabled', !enable);
    $('#cbRole').prop('disabled', !enable);
    $('#cbIdentificationType').prop('disabled', !enable);
    $('#txtIdentificationValue').prop('disabled', !enable);
    $('#txtAddress').prop('disabled', !enable);
    $('#txtEmail').prop('disabled', !enable);
    $('#profilePicture').prop('disabled', !enable);
    $('#btnCancel').prop('disabled', !enable);
    $('#btnSubmit').prop('disabled', !enable);
    $('#cbStatus').prop('disabled', !enable);
}

function UserController() {
    this.ApiService = "user";
    this.title = "Administración de usuarios";

    this.InitView = function () {
        document.title = this.title;

        // Click event for edit button
        $(document).on('click', '.btnEdit', function () {
            let id = $(this).data('id');

            const vc = new UserController();
            vc.RetrieveById(id);

            setTitleModal('Actualizar usuario', 'Actualizar');
            showModal(true);
        });

        // Click event for delete button
        $(document).on('click', '.btnDelete', function () {
            const id = $(this).data('id');
            const vc = new UserController();
            vc.Delete(id);
        });

        // Click event for submit button
        $('#btnSubmit').click(async function () {
            let hideId = $('#txtHideId').val();
            const vc = new UserController();
            if (hideId == null || hideId == "") {
                vc.Create();
            }
            else {
                vc.Update();
            }
        });

        // Click event for show modal button
        $('#btnCreateForm').click(function () {
            setTitleModal('Registrar usuario', 'Registrar');
            showModal(true);
        });

        // Click event for cancel button
        $('#btnCancel').click(function () {
            showModal(false);
            resetFormData();
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

        this.LoadTable();
    };

    this.RetrieveById = function (id) {        
        function successCallback(response) {
            setFormData(response);
        }

        function failCallBack(response) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: response,
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/retrieveById?id=" + id;
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    };


    this.Create = async function () {
        enableDisableForm(false);

        let formData = await readFormData();
        if (!validateFormData(formData)) {
            enableDisableForm(true);
            return;
        }

        function successCallback(response) {
            $('#tblListUsers').DataTable().ajax.reload();
            showModal(false);
            resetFormData();

            // show success message swal
            Swal.fire({
                icon: 'success',
                title: 'Usuario registrado exitosamente',
                text: 'Se ha enviado una contraseña temporal al teléfono del usuario',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);
            enableDisableForm(true);
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/Create";
        controlActions.PostToAPI(serviceRoute, formData, successCallback, failCallBack);
    };

    this.Update = async function () {
        enableDisableForm(false);

        let formData = await readFormData();
        if (!validateFormData(formData)) {
            enableDisableForm(true);
            return;
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/Update";

        function successCallback(response) {
            $('#tblListUsers').DataTable().ajax.reload();
            enableDisableForm(true);
            showModal(false);

            // show success message swal
            Swal.fire({
                icon: 'success',
                title: 'Usuario actualizado exitosamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);
            enableDisableForm(true);
        }

        controlActions.PutToAPI(serviceRoute, formData, successCallback, failCallBack);
    };

    this.Delete = function (id) {
        function successCallBack(response) {
            $('#tblListUsers').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Usuario eliminado exitosamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });            
        };

        function failCallBack(response) {
            $('#tblListUsers').DataTable().ajax.reload();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: response,
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });            
        };

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/delete";
        controlActions.DeleteToAPI(serviceRoute, { id }, successCallBack, failCallBack);
    };

    this.LoadTable = function () {
        var controlActions = new ControlActions();
        var serviceRoute = controlActions.GetUrlApiService(this.ApiService + "/RetrieveAll");

        $('#tblListUsers').DataTable({
            "responsive": true,
            "processing": true,
            "ajax": {
                "url": serviceRoute,
                "dataSrc": function (json) {
                    var roleUser = JSON.parse(sessionStorage.getItem('user')).role;

                    var filteredData = json.filter(function (item) {                        
                        if (roleUser == 'admin') {
                            return true; // return all data
                        }

                        if (roleUser == 'gestor') {
                            return item.role == 'cliente'; // return only clients
                        }
                    });
                    return filteredData;
                },
                "beforeSend": function (request) {
                    request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
                }
            },
            "language": {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
            },
            "columns": [
                { "data": null, "defaultContent": "", "orderable": false, "searchable": false },
                { "data": "firstName", title: "Nombre" },
                { "data": "lastName", title: "Apellidos" },
                { "data": "email", title: "Correo electrónico" },
                {
                    // custom column for role
                    "title": "Rol",
                    "data": null,
                    "render": function (data) {
                        switch (data.role) {
                            case "admin":
                                return "Administrador";
                            case "gestor":
                                return "Gestor";
                            default:
                                return "Cliente";
                        }
                    },
                },
                {
                    // custom column for status
                    "title": "Estado",
                    "data": null,
                    "render": function (data) {
                        switch (data.status) {
                            case 0:
                                return "Eliminado";
                            case 1:
                                return "Activo";
                            default:
                                return "Inactivo";
                        }
                    },
                },
                {
                    // custom column for status
                    "title": "Tipo de identificación",
                    "data": null,
                    "render": function (data) {
                        switch (data.identificationType) {
                            case "national-id	":
                                return "Cédula";
                            case "pasaporte":
                                return "Pasaporte";
                            default:
                                return "Dimex";
                        }
                    }
                },
                { "data": "identificationValue", title: "Número de identificación" },
                { "data": "addressLatitude", title: "Latitud" },
                { "data": "addressLongitude", title: "Longitud" },
                {
                    // Custom column for phone numbers
                    "title": "Teléfonos",
                    "data": null,
                    "render": function (data) {
                        let html = '';
                        for (let i = 0; i < data.phoneNumbers.length; i++) {
                            const phoneNumber = data.phoneNumbers[i];
                            html += phoneNumber + ' ';
                        }
                        return html;
                    }
                },
                { "data": "otpVerified", title: "OTP Verificado" },
                { "data": "isPasswordRequiredChange", title: "Cambiar contraseña" },
                {
                    // Custom column for profile picture
                    "title": "Foto de perfil",
                    "data": null,
                    "render": function (data) {
                        if (data.profilePhoto) {
                            return `<img src="${data.profilePhoto}" class="img-fluid" alt="profile picture" />`;
                        }
                        return '';
                    }
                },
                {
                    "data": "createdDate",
                    "type": "date ",
                    "title": "Fecha de creación",
                    "render": function (value) {
                        return formatDateTime(new Date(value));
                    }
                },
                {
                    "data": "modifiedDate",
                    "type": "date ",
                    "title": "Ultima modificación",
                    "render": function (value) {
                        return formatDateTime(new Date(value));
                    }
                },
                {
                    "orderable": false,
                    'searchable': false,
                    "title": "Acciones",
                    "data": "id",
                    "render": function (data) {
                        var roleUser = JSON.parse(sessionStorage.getItem('user')).role;
                        if (roleUser == "gestor" && data.role == "gestor") {
                            return ''; // gestor can't edit or delete other gestor
                        }

                        return '<button class="btnEdit btn btn-primary m-3 mt-0 mb-0" data-id="' + data + '" >Editar</button>' +
                            '<button class="btnDelete btn btn-danger" data-id="' + data + '">Eliminar</button>';
                    }
                }
            ]
        });
    }
}

$(document).ready(function () {
    var viewCont = new UserController();
    viewCont.InitView();
})