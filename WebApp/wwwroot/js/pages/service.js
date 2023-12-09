let id = 0;
let isEditModal = false;

function readFormData() {
    let formData = { id };
    formData.serviceName = $("#txtServiceName").val();
    formData.serviceDescription = $("#txtServiceDescription").val();
    formData.serviceStatus = $("#txtServiceStatus").val();
    formData.serviceCost = $("#txtServiceCost").val();
    formData.serviceCreatedDate = new Date();
    formData.serviceModifiedDate = new Date();

    return formData;
}

function writeFormData(formData) {
    id = formData.id;
    $("#txtServiceName").val(formData.serviceName);
    $("#txtServiceDescription").val(formData.serviceDescription);
    $("#txtServiceStatus").val(formData.serviceStatus);
    $("#txtServiceCost").val(formData.serviceCost);
    $("#txtServiceCreatedDate").val(formData.serviceCreatedDate);
    $("#txtServiceModifiedDate").val(formData.serviceModifiedDate);
}
function enableFormControls(enabled) {
    $("#txtServiceName").prop("disabled", !enabled);
    $("#txtServiceDescription").prop("disabled", !enabled);
    $("#txtServiceStatus").prop("disabled", !enabled);
    $("#txtServiceCost").prop("disabled", !enabled);
    $("#txtServiceCreatedDate").prop("disabled", !enabled);
    $("#txtServiceModifiedDate").prop("disabled", !enabled);
}
function resetForm() {
    id = 0;
    $("#txtServiceName").val("");
    $("#txtServiceDescription").val("");
    $("#txtServiceStatus").val("");
    $("#txtServiceCost").val("");
    $("#txtServiceCreatedDate").val("");
    $("#txtServiceModifiedDate").val("");

    hideValidationErrors();
    enableFormControls(true);
}
function validateData(formData) {
    hideValidationErrors();

    if (formData.serviceName == "") {
        showValidationErrors("El nombre es requerido");
        return false;
    }

    if (formData.serviceDescription == "") {
        showValidationErrors("La descripción es requerida");
        return false;
    }

    if (formData.serviceStatus == "") {
        showValidationErrors("El estado es requerido");
        return false;
    }

    if (formData.serviceCost == "") {
        showValidationErrors("El costo es requerido");
        return false;
    }

    return true;
}
function ServiceController() {
    this.ApiService = "Service";
    this.Title = "Servicios";

    this.InitView = function () {
        document.Title = this.Title;
        $(document).on('click', '.btnEdit', function () {
            const vc = new ServiceController();
            vc.RetrieveById($(this).data('id'));
        });

        $('#btnCreate').click(function () {
            resetForm();
            isEditModal = false;
            setTitleModal('Registrar servicio', 'Registrar');
            showModal(true);
        });
        $('#btnSubmit').click(function () {
            if (isEditModal) {
                const vc = new ServiceController();
                vc.Update();
            } else {
                const vc = new ServiceController();
                vc.Create();
            }
        });

        $('#btnCancel').click(function () {
            resetForm();
            showModal(false);
        });

        $(document).on('click', '.btnDelete', function () {
            const vc = new ServiceController();
            vc.Delete($(this).data('id'));
        });

        this.LoadTable();
    }

    this.Create = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }


        function successCallback(response) {
            $('#tblListServices').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'El servicio se ha creado correctamente',
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
            $('#tblListServices').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'El servicio se ha actualizado correctamente',
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
            $('#tblListServices').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'El servicio se ha eliminado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        function failCallBack(response) {
            $('#tblListServices').DataTable().ajax.reload();
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
            setTitleModal('Actualizar servicio', 'Actualizar');
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
        columns[0] = { "data": "serviceName", title: "Nombre" };
        columns[1] = { "data": "serviceDescription", title: "Descripción" };
        columns[2] = { "data": "serviceStatus", title: "Estado" };
        columns[3] = { "data": "serviceCost", title: "Costo" };
        columns[4] = {
            "data": "serviceCreatedDate",
            "title": "Fecha de creación",
            "render": function (value) {
                return formatDateTime(new Date(value));
            }
        };
        columns[5] = {
            "data": "serviceModifiedDate",
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

        $('#tblListServices').DataTable({
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
    var viewCont = new ServiceController();
    viewCont.InitView();
});

