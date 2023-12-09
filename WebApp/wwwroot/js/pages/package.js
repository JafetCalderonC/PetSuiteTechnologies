let id = 0;
let isEditModal = false;

function readFormData() {
    let formData = {id};
    formData.packageName = $("#txtName").val();
    formData.description = $("#txtDescription").val();
    formData.roomId = $("#txtRoomId").val();
    formData.petBreedType = $("#txtPetBreedType").val();
    formData.petSize = $("#txtPetSize").val();
    formData.petAggressiveness = $("#txtPetAggressiveness").val();
    formData.status = $("#txtStatus").val();
    formData.services = [];
    formData.CreatedDate = new Date();
    formData.ModifiedDate = new Date();

    $('.service').each(function () {
        let serviceNumber = $(this).val().trim();
        if (serviceNumber) {
            formData.services.push(serviceNumber);
        }
    });

    return formData;
}

function writeFormData(formData) {
    id = formData.id;
    $("#txtName").val(formData.packageName);
    $("#txtDescription").val(formData.description);
    $("#txtRoomId").val(formData.roomId);
    $("#txtPetBreedType").val(formData.petBreedType);
    $("#txtPetSize").val(formData.petSize);
    $("#txtPetAggressiveness").val(formData.petAggressiveness);
    $("#txtStatus").val(formData.status);
    $("#txtCreatedDate").val(formData.CreatedDate);
    $("#txtModifiedDate").val(formData.ModifiedDate);
}

function enableFormControls(enabled) {
    $("#txtName").prop("disabled", !enabled);
    $("#txtDescription").prop("disabled", !enabled);
    $("#txtRoomId").prop("disabled", !enabled);
    $("#txtPetBreedType").prop("disabled", !enabled);
    $("#txtPetSize").prop("disabled", !enabled);
    $("#txtPetAggressiveness").prop("disabled", !enabled);
    $("#txtStatus").prop("disabled", !enabled);
    $("#services").prop("disabled", !enabled);
    $("#txtServiceCreatedDate").prop("disabled", !enabled);
    $("#txtServiceModifiedDate").prop("disabled", !enabled);

}

function resetForm() {
    id = 0;
    $("#txtName").val("");
    $("#txtDescription").val("");
    $("#txtRoomId").val("");
    $("#txtPetBreedType").val("");
    $("#txtPetSize").val("");
    $("#txtPetAggressiveness").val("");
    $("#txtStatus").val("");
    $("#services").val("");
    $("#txtServiceCreatedDate").val("");
    $("#txtServiceModifiedDate").val("");

    hideValidationErrors();
    enableFormControls(true);
}
function validateData(formData) {
    hideValidationErrors();

    if (formData.packageName == "") {
        showValidationErrors("El nombre es requerido");
        return false;
    }

    if (formData.packageDescription == "") {
        showValidationErrors("La descripción es requerida");
        return false;
    }

    if (formData.packageRoomId == "") {
        showValidationErrors("El ID de la habitación es requerido");
        return false;
    }

    if (formData.packagePetBreedType == "") {
        showValidationErrors("El tipo de raza de la mascota es requerido");
        return false;
    }

    if (formData.packagePetSize == "") {
        showValidationErrors("El tamaño de la mascota es requerido");
        return false;
    }

    if (formData.packagePetAggressiveness == "") {
        showValidationErrors("La agresividad de la mascota es requerida");
        return false;
    }

    if (formData.packageStatus == "") {
        showValidationErrors("El estado es requerido");
        return false;
    }

    return true;
}

function PackageController() {

    this.title = "Paquetes";
    this.ApiService = "Package";

    this.InitView = function () {
        document.Title = this.Title;
        $(document).on('click', '.btnEdit', function () {
            const vc = new PackageController();
            vc.RetrieveById($(this).data('id'));
        });

        $('#btnCreate').click(function () {
            resetForm();
            isEditModal = false;
            setTitleModal('Registrar paquete', 'Registrar');
            showModal(true);
        });
        $('#btnSubmit').click(function () {
            if (isEditModal) {
                const vc = new PackageController();
                vc.Update();
            } else {
                const vc = new PackageController();
                vc.Create();
            }
        });

        $('#btnCancel').click(function () {
            resetForm();
            showModal(false);
        });

        $(document).on('click', '.btnDelete', function () {
            const vc = new PackageController();
            vc.Delete($(this).data('id'));
        });

        this.LoadTable();
        this.loadRoomOptions();
    }

    this.Create = async function () {
        enableFormControls(false);

        let formData = await readFormData();
        if (!validateData(formData)) {
            enableFormControls(true);
            return;
        }


        function successCallback(response) {
            $('#tblListPackages').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'El paquete se ha creado correctamente',
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
            $('#tblListPackages').DataTable().ajax.reload();
            showModal(false);

            Swal.fire({
                icon: 'success',
                title: 'El paquete se ha actualizado correctamente',
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
            $('#tblListPackages').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'El servicio se ha eliminado correctamente',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        };

        function failCallBack(response) {
            $('#tblListPackages').DataTable().ajax.reload();
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
            setTitleModal('Actualizar paquete', 'Actualizar');
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
        columns[0] = { "data": "packageName", title: "Nombre" };
        columns[1] = { "data": "description", title: "Descripción" };
        columns[2] = { "data": "roomId", title: "Habitación" };
        columns[3] = { "data": "petBreedType", title: "Raza" };
        columns[4] = { "data": "petSize", title: "Tamaño" };
        columns[5] = { "data": "petAggressiveness", title: "Agresividad" };
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

        $('#tblListPackages').DataTable({
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

    $('#btnAddService').click(function () {
        // Almacena una referencia al objeto actual
        var self = this;

        // add element to service-group
        const serviceGroup = $('#service-group');
        const serviceGroupCount = serviceGroup.children().length;
        const inputID = Date.now();

        // Crea el nuevo select
        let service = $(`
        <div class="input-group mb-2">
            <select id="service-${inputID}" class="service form-control" autocomplete="on"></select>
            <button class="btnRemoveService btn btn-outline-secondary btn-service" type="button">Quitar servicio</button>
        </div>`);

        // Agrega el nuevo select al grupo de servicios
        serviceGroup.append(service);

        self.LoadServicesDropdown();

        // Set focus on the new element
        $(`#service-${inputID}`).focus();

        // Llama a LoadServicesDropdown con el contexto adecuado usando bind

    }.bind(this));  // bind(this) asegura que el contexto sea el objeto actual


    $('#service-group').on('click', '.btnRemoveService', function () {
        // get parent
        let parent = $(this).parent();
        parent.remove();

        let count = 1;
        $('.service').each(function () {
            // change placeholder
            $(this).attr('placeholder', `Servicio ${count++}`);
        });
    });

    this.LoadServicesDropdown = function () {
        var ctrlActions = new ControlActions();
        var urlService = ctrlActions.GetUrlApiService("Service/RetrieveAll");

        $.ajax({
            url: urlService,
            type: 'GET',
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                // Selecciona el último select creado dinámicamente
                var latestSelect = $('#service-group').children().last().find('select');

                // Limpia las opciones actuales
                latestSelect.empty();

                // Agrega las nuevas opciones
                data.forEach(function (service) {
                    latestSelect.append($('<option>', {
                        value: service.id,
                        text: service.serviceName
                    }));
                });
            },
            error: function (error) {
                // Handle error
                console.error('Error loading services:', error);
            }
        });
    };

    this.loadRoomOptions = function () {
        var ctrlActions = new ControlActions();
        var urlRoomOptions = ctrlActions.GetUrlApiService("room/RetrieveAll");

        $.ajax({
            url: urlRoomOptions,
            type: 'GET',
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                var roomSelect = $('#txtRoomId');
                roomSelect.empty();

                data.forEach(function (room) {
                    roomSelect.append($('<option>', {
                        value: room.id,
                        text: room.name
                    }));
                });
            },
            error: function (error) {
                console.error('Error loading room options:', error);
            }
        });
    }

}

//Instanciamiento de la clase
$(document).ready(function () {
    var viewCont = new PackageController();
    viewCont.InitView();
})