
let id = 0
let isEditModal = false;
let petOptions = [];
let packageOptions = [];
let user = sessionStorage.getItem('user');
let userIdLogged = user.IdentificationValue;

function SearchPetId(petOptions, PetName) {
    forEach(petOptions, function (pet) {
        if (pet.PetName == PetName) {
            return pet.Id;
        }

    });
}

//search pet's name
function SearchPetName(petOptions, PetId) {
    forEach(petOptions, function (pet) {
        if (pet.Id == PetId) {
            return pet.PetName;
        }

    });
}

//search package's name
function SearchPackageName(packageOptions, PackageId) {
    forEach(packageOptions, function (package) {
        if (package.Id == PackageId) {
            return package.PackageName;
        }

    });
}


function SearchPackageId(packageOptions, PackageName) {
forEach(packageOptions, function (package) {
        if (package.PackageName == PackageName) {
            return package.Id;
        }

    });
}


function readFormData() {
    let formData = { id };
    formdata.StartDate = $("#StartDate").val();
    formdata.EndDate = $("#EndDate").val();
    formdata.UserID = userIdLogged;
    formdata.PetId = SearchPetId(petOptions, $("#petDropdown").val());
    formdata.PackageId = SearchPackageId(packageOptions, $("#packageDropdown").val());
    formdata.ReservationCreatedDate = new Date();
    formdata.ReservationModifiedDate = new Date();
    
        return formData;
}

function FillDropdowns() {
    // Dropdown de mascotas
    const petDropdown = $('#petDropdown');
    petOptions.forEach(option => {
        petDropdown.append(`<option value="${option.PetName}">${option.PetName}</option>`);
    });

    // Dropdown de paquetes
    const packageDropdown = $('#packageDropdown');
    packageOptions.forEach(option => {
        packageDropdown.append(`<option value="${option.PackageName}">${option.PackageName}</option>`);
    });
}
function writeFormData(formData) {
    id = formData.id;
    $("#StartDate").val(formData.StartDate);
    $("#EndDate").val(formData.EndDate);
    $("#petDropdown").val(SearchPetName(petOptions,formData.PetId)); 
    $("#packageDropdown").val(SearchPackageName(packageOptions,formData.PackageId));
    $("#txtReservationCreatedDate").val(formData.ReservationCreatedDate);
    $("#txtReservationModifiedDate").val(formData.ReservationModifiedDate);
}

function enableFormControls(enabled) {
    $("#StartDate").prop("disabled", !enabled);
    $("#EndDate").prop("disabled", !enabled);
    $("#txtUserID").prop("disabled", !enabled);
    $("#petDropdown").prop("disabled", !enabled);
    $("#packageDropdown").prop("disabled", !enabled);
    $("#txtReservationCreatedDate").prop("disabled", !enabled);
    $("#txtReservationModifiedDate").prop("disabled", !enabled);
}

function resetForm() {
    id = 0;
    $("#StartDate").val("");
    $("#EndDate").val("");
    $("#txtUserID").val("");
    $("#petDropdown").val("");
    $("#packageDropdown.").val("");
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
        RetrievePetByUserID(userIdLogged);
        RetrieveAllPackages();
        this.LoadTable();
    }

    function RetrievePetByUserID(userIdLogged)
    {
        function successCallback(response) {
            petOptions = response.Data;
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
        const serviceRoute = "Pet" + "/RetrieveByUserID?userID=" + userIdLogged;
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }

    // Package/RetrieveAll
    function RetrieveAllPackages()
    {
        function successCallback(response) {
            packageOptions = response.Data;
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
        columns[0] = { "data": "StartDate", title: "Fecha de inicio" };
        columns[1] = { "data": "EndDate", title: "Fecha de fin" };
        columns[2] = { "data": "UserID", title: "Usuario" };
        columns[3] = { "data": "PetId", title: "Mascota" };
        columns[4] = { "data": "PackageId", title: "Paquete" };
        columns[5] = {
            "data": "ReservationCreatedDate",
            "title": "Fecha de creacion",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[6] = {
            "data": "ReservationModifiedDate",
            "title": "Fecha de modificacion",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[7] = {
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

