let id = 0;
let isEditModal = false;

function readFormData() {
    let formData = { id };
    formData.petName = $("#txtName").val();
    formData.description = $("#txtDescription").val();
    formData.petAge = $("#txtAge").val();
    formData.petBreedType = $("#txtPetBreedType").val();
    formData.petAggressiveness = $("#txtPetAggressiveness").val();
    formData.userId = $("#txtUserId").val();
    formData.status = $("#txtStatus option:selected").val();
    formData.createdDate = new Date();
    formData.modifiedDate = new Date();

    return formData;
}

function writeFormData(formData) {
    id = formData.id;
    $("#txtName").val(formData.petName);
    $("#txtDescription").val(formData.description);
    $("#txtAge").val(formData.petAge);
    $("#txtPetBreedType").val(formData.petBreedType);
    $("#txtPetAggressiveness").val(formData.petAggressiveness);
    $("#txtUserId").val(formData.userId);
    $("#txtStatus").val(formData.status);
    $("#txtCreatedDate").val(formData.createdDate);
    $("#txtModifiedDate").val(formData.modifiedDate);
}

function enableFormControls(enabled) {
    $("#txtName").prop("disabled", !enabled);
    $("#txtDescription").prop("disabled", !enabled);
    $("#txtAge").prop("disabled", !enabled);
    $("#txtPetBreedType").prop("disabled", !enabled);
    $("#txtPetAggressiveness").prop("disabled", !enabled);
    $("#txtUserId").prop("disabled", !enabled);
    $("#txtStatus").prop("disabled", !enabled);
    $("#txtCreatedDate").prop("disabled", !enabled);
    $("#txtModifiedDate").prop("disabled", !enabled);

}

function resetForm() {
    id = 0;
    $("#txtName").val("");
    $("#txtDescription").val("");
    $("#txtAge").val("");
    $("#txtPetBreedType").val("");
    $("#txtPetAggressiveness").val("");
    $("#txtUserId").val("");
    $("#txtStatus").val("");

    $("#txtCreatedDate").val("");
    $("#txtModifiedDate").val("");
    hideValidationErrors();
    enableFormControls(true);
}
function validateData(formData) {
    hideValidationErrors();

    if (formData.petName === "") {
        showValidationErrors("El nombre de la mascota es requerido");
        return false;
    }

    if (formData.description === "") {
        showValidationErrors("La descripción de la mascota es requerida");
        return false;
    }

    if (formData.petAge === "") {
        showValidationErrors("La edad de la mascota es requerida");
        return false;
    }

    if (formData.petBreedType === "") {
        showValidationErrors("El tipo de raza de la mascota es requerido");
        return false;
    }

    if (formData.petAggressiveness === "") {
        showValidationErrors("La agresividad de la mascota es requerida");
        return false;
    }

    if (formData.userId === "") {
        showValidationErrors("El ID de usuario es requerido");
        return false;
    }

    if (formData.status === "") {
        showValidationErrors("El estado es requerido");
        return false;
    }

    return true;
}

function PetController() {

    this.title = "Pets";
    this.ApiService = "Pet";

    this.InitView = function () {
        document.Title = this.Title;
        $(document).on('click', '.btnEdit', function () {
            const vc = new PetController();
            vc.RetrieveById($(this).data('id'));
        });

        $('#btnCreate').click(function () {
            resetForm();
            isEditModal = false;
            setTitleModal('Registrar mascota', 'Registrar');
            showModal(true);
        });
        $('#btnSubmit').click(function () {
            if (isEditModal) {
                const vc = new PetController();
                vc.Update();
            } else {
                const vc = new PetController();
                vc.Create();
            }
        });

        $('#btnCancel').click(function () {
            resetForm();
            showModal(false);
        });

        $(document).on('click', '.btnDelete', function () {
            const vc = new PetController();
            vc.Delete($(this).data('id'));
        });

        this.LoadTable();
        this.loadUserOptions();
    }

    this.Create = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }


        function successCallback(response) {
            $('#tblListPets').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La mascota se ha creado correctamente',
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

    this.Update = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }

        function successCallback(response) {
            $('#tblListPets').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La mascota se ha actualizado correctamente',
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


    };
    this.Delete = function (id) {
        function successCallBack(response) {
            $('#tblListPets').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'El mascotas se ha eliminado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        function failCallBack(response) {
            $('#tblListPets').DataTable().ajax.reload();
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
    };

    this.RetrieveById = function (id) {
        resetForm();

        function successCallback(response) {
            isEditModal = true;
            writeFormData(response);
            setTitleModal('Actualizar mascota', 'Actualizar');
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
        var ctrlActions = new ControlActions();
        var urlService = ctrlActions.GetUrlApiService(this.ApiService + "/RetrieveAll")

        let columns = [];
        columns[0] = { "data": "petName", title: "Nombre" };
        columns[1] = { "data": "description", title: "Descripción" };
        columns[2] = { "data": "petAge", title: "Edad" };
        columns[3] = { "data": "petBreedType", title: "Raza" };
        columns[4] = { "data": "petAggressiveness", title: "Agresividad" };
        columns[5] = { "data": "userId", title: "Id de Usuario" };
        columns[6] = { "data": "status", title: "Estado" };

        columns[7] = {
            "data": "createdDate",
            "title": "Fecha de creación",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[8] = {
            "data": "modifiedDate",
            "title": "Fecha de modificación",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[9] = {
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

        $("#tblListPets").DataTable({
            "ajax": {
                "url": urlService,
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

    this.loadUserOptions = function () {
        var ctrlActions = new ControlActions();
        var urlUserOptions = ctrlActions.GetUrlApiService("User/RetrieveAll");

        $.ajax({
            url: urlUserOptions,
            type: 'GET',
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                var userSelect = $('#txtUserId');
                userSelect.empty();

                data.forEach(function (user) {
                    userSelect.append($('<option>', {
                        value: user.id,
                        text: user.identificationValue
                    }));
                });
            },
            error: function (error) {
                console.error('Error loading user options:', error);
            }
        });
    }

}

//Instanciamiento de la clase
$(document).ready(function () {
    var viewCont = new PetController();
    viewCont.InitView();
})