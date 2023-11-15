function ServiceController() {

    this.ApiService = "Service";

    this.InitView = function () {

        $(".bs-component form").hide();
        $("#btnUpdate").hide();
        $("#btnDelete").hide();


        $("#tblService").show();


        $("#btnToggleForm").click(function () {
            $(".bs-component form").toggle();
            $("#btnUpdate").toggle();
            $("#btnDelete").toggle();
            $("#tblService").toggle(); 
        });

        $("#btnCreate").click(function () {
            var vc = new ServiceController();
            vc.Create();
        });

        $("#btnUpdate").click(function () {
            var vc = new ServiceController();
            vc.Update();
        });

        this.LoadTable();
    };

    this.ValidateInputs = function () {
        var serviceName = $("#txtServiceName").val();
        var serviceDescription = $("#txtServiceDescription").val();
        var serviceStatus = $("#txtServiceStatus").val();
        var serviceCost = $("#txtServiceCost").val();

        if (!serviceName) {
            Swal.fire("Error", "Service Name cannot be empty.", "error");
            return false;
        }
        if (!serviceDescription) {
            Swal.fire("Error", "Service Description cannot be empty.", "error");
            return false;
        }
        if (!serviceStatus) {
            Swal.fire("Error", "Service Status cannot be empty.", "error");
            return false;
        }
        if (!serviceCost || parseFloat(serviceCost) <= 0) {
            Swal.fire("Error", "Service Cost must be a number greater than 0.", "error");
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

        controlActions.PostToAPI(serviceRoute, service, function () { console.log("Service created" + JSON.stringify(service)) });


    }
    this.Update = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var service = {};
        service.serviceName = $("#txtServiceName").val();
        service.serviceDescription = $("#txtServiceDescription").val();
        service.serviceStatus = $("#txtServiceStatus").val();
        service.serviceCost = $("#txtServiceCost").val();
        service.serviceModifiedDate = new Date();

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Update";
        controlActions.PutToAPI(serviceRoute, service, function () { console.log("Service updated" + JSON.stringify(service)) });

    }
    this.Delete = function () {
        var service = {};
        service.service.id = $("#txtServiceId").val();
        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Delete";
        controlActions.PostToAPI(serviceRoute, service, function () { console.log("Service deleted" + JSON.stringify(service)) });

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
                "dataSrc": ""
            },
            "columns": columns
        });

    }


}


// Llama a la función al cargar la página
$(document).ready(function () {
    var viewCont = new ServiceController();
    viewCont.InitView();
});