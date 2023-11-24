let id = 0;
let isEditModal = false;

// Read from form
function readFormData() {
    let formData = { id };
    formData.name = $('#txtName').val().trim();
    formData.description = $('#txtDescription').val().trim();
    formData.cost = $('#txtCost').val().trim();
    formData.status = $('#cbStatus option:selected').val();
    
    return formData;
}

// Write data to form
function writeFormData(formData) {
    id = formData.id;
    $('#txtName').val(formData.name);
    $('#txtDescription').val(formData.description);
    $('#txtCost').val(formData.cost);
    $('#cbStatus').val(formData.status);
}

// Enable or disable form controls
function enableFormControls(enabled) {
    $('#txtName').prop('disabled', !enabled);
    $('#txtDescription').prop('disabled', !enabled);
    $('#txtCost').prop('disabled', !enabled);
    $('#cbStatus').prop('disabled', !enabled);
}

// reset form
function resetForm() {
    id = 0;
    $('#txtName').val('');
    $('#txtDescription').val('');
    $('#txtCost').val('');
    $('#cbStatus').val('');

    hideValidationErrors();
    enableFormControls(true);
}

// Validate data
function validateData(formData) {
    hideValidationErrors();

    if (formData.name == '') {
        showValidationErrors('El nombre es requerido');
        return false;
    }

    if (formData.description == '') {
        showValidationErrors('La descripción es requerida');
        return false;
    }

    if (formData.cost == 0) {
        showValidationErrors('El costo es requerido');
        return false;
    }

    if (formData.status == '') {
        showValidationErrors('El estado es requerido');
        return false;
    }

    return true;
}



function RoomController() {
    this.ApiService = "room";
    this.title = "Administración de habitaciones";

    this.InitView = function () {
        document.title = this.title;

        // Show modal to edit
        $(document).on('click', '.btnEdit', function () {
            const vc = new RoomController();
            vc.RetrieveById($(this).data('id'));
        });

        // Show modal to create
        $('#btnCreate').click(function () {
            resetForm();
            isEditModal = false;
            setTitleModal('Registrar habitación', 'Registrar');
            showModal(true);
        });

        // Button submit form
        $('#btnSubmit').click(function () {
            if (isEditModal) {
                const vc = new RoomController();
                vc.Update();
            } else {
                const vc = new RoomController();
                vc.Create();
            }
        });

        // Button cancel form
        $('#btnCancel').click(function () {
            resetForm();
            showModal(false);
        });

        // Button delete 
        $(document).on('click', '.btnDelete', function () {
            const vc = new RoomController();
            vc.Delete($(this).data('id'));
        });

        this.LoadTable();
    };


    this.Create = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }

        function successCallback(response) {
            $('#tblListRooms').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La habitación creada correctamente',
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
    };

    this.Update = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }

        function successCallback(response) {
            $('#tblListRooms').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'La habitación se ha actualizado correctamente',
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
            $('#tblListRooms').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'La habitación se ha eliminado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        function failCallBack(response) {
            $('#tblListRooms').DataTable().ajax.reload();
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
            setTitleModal('Actualizar habitación', 'Actualizar');
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
    };

    this.LoadTable = function () {
        let controlActions = new ControlActions();
        let serviceRoute = controlActions.GetUrlApiService(this.ApiService + "/RetrieveAll");

        let columns = [];
        columns[0] = { "data": "name", title: "Nombre" };
        columns[1] = { "data": "description", title: "Descripción" };
        columns[2] = { "data": "cost", title: "Costo" };
        columns[3] = {
            "title": "Estado",
            "data": "status",
            "render": function (value) {
                switch (value) {
                    case 0:
                        return "Eliminado";
                    case 1:
                        return "Activo";
                    default:
                        return "Inactivo";
                }
            },
        };
        columns[4] = {
            "data": "createdDate",
            "title": "Fecha de creación",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[5] = {
            "data": "modifiedDate",
            "title": "Fecha de modificación",
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

        $('#tblListRooms').DataTable({
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
    };
}

$(document).ready(function () {
    var viewCont = new RoomController();
    viewCont.InitView();
});