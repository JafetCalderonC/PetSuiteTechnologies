
let id = 0
let isEditModal = false;
let petOptions = [];
let packageOptions = [];

// return user logged id

function UserLogged() {
    let user = JSON.parse(sessionStorage.getItem('user'));
    userIdLogged = user.id;
    return userIdLogged;
}

function SearchPetId(petOptions, petName) {
    for (let i = 0; i < petOptions.length; i++) {
        if (petOptions[i].petName === petName) {
            return petOptions[i].id;
        }
    }
   
    return null;
}



function SearchPetName(petOptions, petId) {
    for (let i = 0; i < petOptions.length; i++) {
        if (petOptions[i].id === petId) {
            return petOptions[i].petName;
        }
    }
    return null; 
}

function SearchPackageName(packageOptions, packageId) {
    for (let i = 0; i < packageOptions.length; i++) {
        if (packageOptions[i].id === packageId) {
            return packageOptions[i].packageName;
        }
    }
    return null; 
}



function SearchPackageId(packageOptions, packageName) {
    for (let i = 0; i < packageOptions.length; i++) {
        if (packageOptions[i].packageName === packageName) {
            return packageOptions[i].id;
        }
    }
    return null; 
}


function readFormData() {
    let formData = {id};
    formData.startDate = $("#startDate").val();
    formData.endDate = $("#endDate").val();
    formData.userID = UserLogged();
    formData.petId = SearchPetId(petOptions, $("#petDropdown").val());
    formData.packageId = SearchPackageId(packageOptions, $("#packageDropdown").val());
    formData.reservationCreatedDate = new Date();
    formData.reservationModifiedDate = new Date();
    
        return formData;
}

function FillDropdowns() {
    // Dropdown de mascotas
    const petDropdown = $('#petDropdown');
    petOptions.forEach(option => {
        petDropdown.append(`<option value="${option.petName}">${option.petName}</option>`);
    });

    // Dropdown de paquetes
    const packageDropdown = $('#packageDropdown');
    packageOptions.forEach(option => {
        packageDropdown.append(`<option value="${option.packageName}">${option.packageName}</option>`);
    });
}
function writeFormData(formData) {
    id = formData.id;
    $("#startDate").val(formData.StartDate);
    $("#endDate").val(formData.EndDate);
    $("#petDropdown").val(SearchPetName(petOptions,formData.PetId)); 
    $("#packageDropdown").val(SearchPackageName(packageOptions,formData.PackageId));
    $("#txtReservationCreatedDate").val(formData.ReservationCreatedDate);
    $("#txtReservationModifiedDate").val(formData.ReservationModifiedDate);
}

function enableFormControls(enabled) {
    $("#startDate").prop("disabled", !enabled);
    $("#endDate").prop("disabled", !enabled);
    $("#txtUserID").prop("disabled", !enabled);
    $("#petDropdown").prop("disabled", !enabled);
    $("#packageDropdown").prop("disabled", !enabled);
    $("#txtReservationCreatedDate").prop("disabled", !enabled);
    $("#txtReservationModifiedDate").prop("disabled", !enabled);
}

function resetForm() {
    id = 0;
    $("#startDate").val("");
    $("#endDate").val("");
    $("#txtUserID").val("");
    $("#petDropdown").val("");
    $("#packageDropdown").val("");
    $("#txtReservationCreatedDate").val("");
    $("#txtReservationModifiedDate").val("");

    hideValidationErrors();
    enableFormControls(true);
}

function validateData(formData) {
    hideValidationErrors();

    if (formData.StartDate == "") {
        showValidationErrors("La fecha de inicio es requerida");
        return false;
    }

    if (formData.EndDate == "") {
        showValidationErrors("La fecha de fin es requerida");
        return false;
    }

    if (formData.UserID == "") {
        showValidationErrors("El usuario es requerido");
        return false;
    }

    if (formData.PetId == "") {
        showValidationErrors("La mascota es requerida");
        return false;
    }

    if (formData.PackageId == "") {
        showValidationErrors("El paquete es requerido");
        return false;
    }

    return true;
}

function ReservationController() {
    this.ApiService = "Reservation";
    this.Title = "Reservaciones";

    this.InitView = function () {
        document.Title = this.Title;
        petOptions = [];
        packageOptions = [];
        $(document).on('click', '.btnEdit', function () {
            const vc = new ReservationController();
            vc.RetrieveById($(this).data('id'));
        });

        $('#btnCreate').click(function () {
            resetForm();
            isEditModal = false;
            setTitleModal('Registrar reservación', 'Registrar');
            showModal(true);
        });
        $('#btnSubmit').click(function () {
            if (isEditModal) {
                const vc = new ReservationController();
                vc.Update();
            } else {
                const vc = new ReservationController();
                vc.Create();
            }
        });

        $('#btnCancel').click(function () {
            resetForm();
            showModal(false);
        });

        $(document).on('click', '.btnDelete', function () {
            const vc = new ReservationController();
            vc.Delete($(this).data('id'));
        });
        RetrievePetByUserID(UserLogged());
        RetrieveAllPackages();
        this.LoadTable();
    }

    function RetrievePetByUserID(userIdLogged)
    {
        function successCallback(response) {
            petOptionsData = response;
            petOptionsData.forEach(function (obj) {
                var newObj = { id: obj.id, petName: obj.petName };
                petOptions.push(newObj);
            });

     
            FillDropdowns();
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
        const serviceRoute = "Pet" + "/RetrieveByUserId?id=" + userIdLogged;
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }

    function RetrieveAllPackages()
    {
        function successCallback(response) {


            packageOptionsResponse = response;
            packageOptionsResponse.forEach(function (obj) {
                var newObj = { id: obj.id, packageName: obj.packageName };
                packageOptions.push(newObj);
            });

            FillDropdowns();
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
        const serviceRoute = "Package" + "/RetrieveAll";
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }
    this.Create = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }

        function successCallback(response) {
            $('#tblListReservations').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La reservación se ha creado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });

        }

        function failCallBack(response) {
            showValidationErrors(response);
            enableFormControls(true);
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/Create";
        controlActions.PostToAPI(serviceRoute, formData, successCallback, failCallBack);

    }

    // update reservation
    this.Update = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }

        function successCallback(response) {
            $('#tblListReservations').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La reservación se ha actualizado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);
            enableFormControls(true);
        }

        const controlActions = new ControlActions();
        const serviceRoute = this.ApiService + "/Update";
        controlActions.PutToAPI(serviceRoute, formData, successCallback, failCallBack);
    }

    this.Delete = function (id) {
    function successCallBack(response) {
            $('#tblListReservations').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'La reservación se ha eliminado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        function failCallBack(response) {
            $('#tblListReservations').DataTable().ajax.reload();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: response,
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/delete";
        controlActions.DeleteToAPI(serviceRoute, { id }, successCallBack, failCallBack);
    }

    this.RetrieveById = function (id) {
        resetForm();

        function successCallback(response) {
            isEditModal = true;
            writeFormData(response);
            setTitleModal('Actualizar reservación', 'Actualizar');
            showModal(true);
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
        const serviceRoute = this.ApiService + "/RetrieveById?id=" + id;
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);

    }

    this.LoadTable = function () {
    let controlActions = new ControlActions();
        let serviceRoute = controlActions.GetUrlApiService(this.ApiService + "/RetrieveAll");

        let columns = [];
        columns[0] = { "data": "startDate", title: "Fecha de inicio" };
        columns[1] = { "data": "endDate", title: "Fecha de fin" };
        columns[2] = {
            "data": "petId",
            "title": "Mascota",
            "render": function (petId) {
                //RetrievePetByUserID(UserLogged());
                let petName = SearchPetName(petOptions, petId); 
                return petName; 
            }
        };
        columns[3] = {
            "data": "packageId",
            "title": "Paquete",
            "render": function (packageId) {
                //RetrieveAllPackages();
                let packageName = SearchPackageName(packageOptions, packageId);
                return packageName;
            }
        };
        columns[4] = {
            "data": "reservationCreatedDate",
            "title": "Fecha de creacion",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[5] = {
            "data": "reservationModifiedDate",
            "title": "Fecha de modificacion",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[6] = {
            "orderable": false,
            'searchable': false,
            "title": "Acciones",
            "data": "id",
            "render": function (value) {
                return '<div style="display: flex;">' +
                    '<button class="btnEdit btn btn-primary m-3 mt-0 mb-0" data-id="' + value + '" >Editar</button>' +
                    '<button class="btnDelete btn btn-danger" data-id="' + value + '">Eliminar</button>' +
                    '</div>';
            }
        };

        $('#tblListReservations').DataTable({
            "responsive": true,
            "processing": true,
            "ajax": {
                "url": serviceRoute,
                "dataSrc": "",
                "beforeSend": function (request) {
                    request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
                }
            },
            "columns": columns,
            "language": {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
            },
        });
    }




}

$(document).ready(function () {
    var viewCount = new ReservationController();
    viewCount.InitView();

});

