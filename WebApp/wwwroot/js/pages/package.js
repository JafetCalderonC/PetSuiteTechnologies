//Clase JS que es el controlador de la vista.
//Users.cshtml

//Definicion de la clase
function PackageController() {

    this.ViewName = "Package";
    this.ApiService = "PackageCRUD";

    this.InitView = function () {

        console.log("City view init!!!");
        //Binding del evento del clic al metodo de create del controlador
        $("#btnCreate").click(function () {
            var vc = new PackageController();
            vc.Create();
        })

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

        ctrlActions.PostToAPI(serviceRoute, package, function () {
            console.log("Package created --> " + JSON.stringify(package));
        });
    }
}


    //Instanciamiento de la clase
    $(document).ready(function () {
        var viewCont = new PackageController();
        viewCont.InitView();
    })