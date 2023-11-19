function ServiceController() {

    this.ApiService = "Service";
    this.title = "Servicios";

    this.InitView = function () {
        document.title = this.title;


        $(".bs-component form").hide();
        $("#btnUpdate").hide();
        $("#btnDelete").hide();


        $("#divtblService").show();

        
        $("#btnToggleForm").click(function () {
            $(".bs-component form").toggle();
            $("#btnUpdate").toggle();
            $("#btnDelete").toggle();
            $("#divtblService").toggle();
        });

        $("#btnCreate").click(function () {
            var vc = new ServiceController();
            vc.Create();
        });

        $("#btnUpdate").click(function () {
            var vc = new ServiceController();
            vc.Update();
        });
        $("#btnDelete").click(function () {
            var vc = new ServiceController();
            vc.Delete();
        })

        this.LoadTable();
    };

    this.ValidateInputs = function () {
        var serviceName = $("#txtServiceName").val();
        var serviceDescription = $("#txtServiceDescription").val();
        var serviceStatus = $("#txtServiceStatus").val();
        var serviceCost = $("#txtServiceCost").val();

        if (!serviceName) {
            Swal.fire("Error", "El nombre del servicio no puede estar vacío.", "error");
            return false;
        }
        if (!serviceDescription) {
            Swal.fire("Error", "La descripción no puede estar vacía.", "error");
            return false;
        }
        if (!serviceStatus) {
            Swal.fire("Error", "El estado no puede estar vacío.", "error");
            return false;
        }
        if (!serviceCost || parseFloat(serviceCost) <= 0) {
            Swal.fire("Error", "El costo no puede estar vacío o ser menor o igual a 0.", "error");
            return false;
        }

        return true;
    }

    this.Create = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var service = {};
        service.serviceName = $("#txtServiceName").val();
        service.serviceDescription = $("#txtServiceDescription").val();
        service.serviceStatus = $("#txtServiceStatus").val();
        service.serviceCost = $("#txtServiceCost").val();
        service.serviceCreatedDate = new Date();
        service.serviceModifiedDate = new Date();

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Create";

        function successCallback() {
            Swal.fire("Success", "El servicio se ha creado correctamente.", "success");
            // reload
            location.reload();
        }

        function failCallback(response) {
            Swal.fire("Error", response, "error");
        }


        controlActions.PostToAPI(serviceRoute, service, successCallback, failCallback);
    }

    this.Update = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var service = {};
        service.Id = $("#txtServiceId").val();
        service.serviceName = $("#txtServiceName").val();
        service.serviceDescription = $("#txtServiceDescription").val();
        service.serviceStatus = $("#txtServiceStatus").val();
        service.serviceCost = $("#txtServiceCost").val();
        service.serviceModifiedDate = new Date();

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Update";

        function successCallback() {
            Swal.fire("Success", "El servicio se ha actualizado correctamente.", "success");
            // reload
            location.reload();
        }

        function failCallback(response) {
            Swal.fire("Error", response, "error");
        }

        controlActions.PutToAPI(serviceRoute, service, successCallback, failCallback);

    }
    this.Delete = function () {
        var service = {};
        service.Id = +$("#txtServiceId").val();
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

        controlActions.DeleteToAPI(serviceRoute, service, successCallback, failCallback);
    }

    this.LoadTable = function () {
        var controlActions = new ControlActions();
        var urlService = controlActions.GetUrlApiService(this.ApiService + "/RetrieveAll");

        var columns = [];
        columns[0] = { 'data': 'id' };
        columns[1] = { 'data': 'serviceName' };
        columns[2] = { 'data': 'serviceDescription' };
        columns[3] = { 'data': 'serviceStatus' };
        columns[4] = { 'data': 'serviceCost' };
        columns[5] = { 'data': 'serviceCreatedDate' };
        columns[6] = { 'data': 'serviceModifiedDate' };
        $("#tblService").DataTable({
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
}


// Llama a la función al cargar la página
$(document).ready(function () {
    var viewCont = new ServiceController();
    viewCont.InitView();
});