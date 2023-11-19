//Clase JS que es el controlador de la vista.
//Users.cshtml

//Definicion de la clase
function PackageController() {

    this.title = "Paquetes";
    this.ApiService = "Package";

    this.InitView = function () {
        document.title = this.title;

        console.log("City view init!!!");
        //Binding del evento del clic al metodo de create del controlador
        $("#btnCreate").click(function () {
            var vc = new PackageController();
            vc.Create();
        })


        this.LoadTable();


        //$("#btnUpdate").click(function () {
        //    var vc = new PackageController();
        //    vc.Update();
        //})

        //$("#btnDelete").click(function () {
        //    var vc = new PackageController();
        //    vc.Delete();
        //})


    }
    this.Create = function () {

        var package = {};
        package.PackageName = $("#txtName").val();
        package.Description = $("#txtDescription").val();
        package.RoomId = $("#txtRoomId").val();
        package.PetBreedType = $("#txtPetBreedType").val();
        package.PetSize = $("#txtPetSize").val();
        package.PetAggressiveness = $("#txtPetAggressiveness").val();
        package.Status = $("#txtStatus").val(); txtServices
        package.Services = $("#txtServices").val().split(',').map(s => s.trim());

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
    this.LoadTable = function () {
        var ctrlActions = new ControlActions();
        var urlService = ctrlActions.GetUrlApiService(this.ApiService + "/RetrieveAll")

        var columns = []
        columns[0] = { 'data': 'package_id' }
        columns[1] = { 'data': 'package_name' }
        columns[2] = { 'data': 'Description' }
        columns[3] = { 'data': 'room_id' }
        columns[4] = { 'data': 'pet_breed_type' }
        columns[5] = { 'data': 'pet_size' }
        columns[6] = { 'data': 'pet_aggressiveness' }
        columns[7] = { 'data': 'created_date' }
        columns[8] = { 'data': 'modified_date' }
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


}


//Instanciamiento de la clase
$(document).ready(function () {
    var viewCont = new PackageController();
    viewCont.InitView();
})