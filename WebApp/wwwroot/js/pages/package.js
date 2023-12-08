function PackageController() {

    this.title = "Paquetes";
    this.ApiService = "Package";

    this.InitView = function () {
        document.title = this.title;
        
        $(".bs-component form").hide();
        $("#btnUpdate").hide();
        $("#btnDelete").hide();
        $("#btnCreate").hide();

        $("#tblListPackages").show();

        $("#btnToggleForm").click(function () {
            $(".bs-component form").toggle();
            $("#btnCreate").toggle();
            $("#btnUpdate").toggle();
            $("#btnDelete").toggle();
            $("#tblListPackages").toggle();
        });

        $("#btnCreate").click(function () {
            var vc = new PackageController();
            vc.Create();
        });
        $("#btnUpdate").click(function () {
            var vc = new PackageController();
            vc.Update();
        });
        $("#btnDelete").click(function () {
            var vc = new PackageController();
            vc.Delete();
        });

        this.LoadTable();
        this.loadRoomOptions();
    }


    this.ValidateInputs = function () {
        var packageName = $("#txtName").val();
        var packageDescription = $("#txtDescription").val();
        var packageRoomId = $("#txtRoomId").val();
        var packagePetBreedType = $("#txtPetBreedType").val();
        var packagePetSize = $("#txtPetSize").val();
        var packagePetAggressiveness = $("#txtPetAggressiveness").val();
        var packageStatus = $("#txtStatus").val();
        var packageServices = [];
        $('.service').each(function () {
            let serviceNumber = $(this).val().trim();
            if (serviceNumber) {
                packageServices.push(serviceNumber);
            }
        });

        if (!packageName) {
            Swal.fire("Error", "El nombre del paquete no puede estar vacío.", "error");
            return false;
        }
        if (!packageDescription) {
            Swal.fire("Error", "La descripción del paquete no puede estar vacía.", "error");
            return false;
        }
        if (!packageRoomId) {
            Swal.fire("Error", "la habitacion del paquete no puede estar vacía.", "error");
            return false;
        }
        if (!packagePetBreedType) {
            Swal.fire("Error", "El tipo de raza del paquete no puede estar vacío.", "error");
            return false;
        }
        if (!packagePetSize) {
            Swal.fire("Error", "El tamaño del paquete no puede estar vacío.", "error");
            return false;
        }
        if (!packagePetAggressiveness) {
            Swal.fire("Error", "La agresividad del paquete no puede estar vacía.", "error");
            return false;
        }
        if (!packageStatus) {
            Swal.fire("Error", "El estado del paquete no puede estar vacío.", "error");
            return false;
        }
        if (!packageServices) {
            Swal.fire("Error", "El paquete requiere de almenos un servicio", "error");
            return false;
        }

        return true;
    }

    this.Create = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var package = {};
        package.PackageName = $("#txtName").val();
        package.Description = $("#txtDescription").val();
        package.RoomId = $("#txtRoomId").val();
        package.PetBreedType = $("#txtPetBreedType").val();
        package.PetSize = $("#txtPetSize").val();
        package.PetAggressiveness = $("#txtPetAggressiveness").val();
        package.Status = $("#txtStatus").val();
        package.Services = [];
        package.CreatedDate = new Date();
        package.ModifiedDate = new Date();

        $('.service').each(function () {
            let serviceNumber = $(this).val().trim();
            if (serviceNumber) {
                package.Services.push(serviceNumber);
            }
        });

        var ctrlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Create";

        function successCallback(response) {
            Swal.fire("success!", "Paquete creado correctamente!", "success");
        }

        function failCallback(response, status) {
            Swal.fire("error!",response, "error");
            console.log("fail callback");
        }


        ctrlActions.PostToAPI(serviceRoute, package, successCallback, failCallback);
    }

    this.Update = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var package = {};
        package.Id = $("#txtPackageId").val();
        package.PackageName = $("#txtName").val();
        package.Description = $("#txtDescription").val();
        package.RoomId = $("#txtRoomId").val();
        package.PetBreedType = $("#txtPetBreedType").val();
        package.PetSize = $("#txtPetSize").val();
        package.PetAggressiveness = $("#txtPetAggressiveness").val();
        package.Status = $("#txtStatus").val();
        package.Services = [];
        package.CreatedDate = new Date();
        package.ModifiedDate = new Date();

        $('.service').each(function () {
            let serviceNumber = $(this).val().trim();
            if (serviceNumber) {
                package.Services.push(serviceNumber);
            }
        });

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Update";

        function successCallback(response) {
            Swal.fire("success!", "Paquete modificadp correctamente!", "success");
        }

        function failCallback(response, status) {
            Swal.fire("error!", response, "error");
            console.log("fail callback");
        }

        controlActions.PutToAPI(serviceRoute, package, successCallback, failCallback);

    }
    this.Delete = function () {
        var package = {};
        package.Id = +$("#txtPackageId").val();
        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Delete";

        function successCallback() {
            Swal.fire("Success", "El servicio se ha eliminado correctamente.", "success");
            // reload
            location.reload();
        }

        function failCallback(response) {
            Swal.fire("Error", response, "error");
        }

        controlActions.DeleteToAPI(serviceRoute, package, successCallback, failCallback);
    }
    this.LoadTable = function () {
        var ctrlActions = new ControlActions();
        var urlService = ctrlActions.GetUrlApiService(this.ApiService + "/RetrieveAll")

        var columns = []
        columns[0] = { 'data': 'id' }
        columns[1] = { 'data': 'packageName' }
        columns[2] = { 'data': 'description' }
        columns[3] = { 'data': 'roomId' }
        columns[4] = { 'data': 'petBreedType' }
        columns[5] = { 'data': 'petSize' }
        columns[6] = { 'data': 'petAggressiveness' }
        columns[7] = { 'data': 'createdDate' }
        columns[8] = { 'data': 'modifiedDate' }
        columns[9] = { 'data': 'status' }

        $("#tblListPackages").DataTable({
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

    this.function loadRoomOptions = function () {
        var ctrlActions = new ControlActions();
        var urlRoomOptions = ctrlActions.GetUrlApiService("Room/RetrieveAll");

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
                        text: room.roomName
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