function PetController() {

    this.title = "Pets";
    this.ApiService = "Pet";

    this.InitView = function () {
        document.title = this.title;
        
        $(".bs-component form").hide();
        $("#btnUpdate").hide();
        $("#btnDelete").hide();
        $("#btnCreate").hide();

        $("#tblListPets").show();

        $("#btnToggleForm").click(function () {
            $(".bs-component form").toggle();
            $("#btnCreate").toggle();
            $("#btnUpdate").toggle();
            $("#btnDelete").toggle();
            $("#tblListPets").toggle();
        });

        $("#btnCreate").click(function () {
            var vc = new PetController();
            vc.Create();
        });
        $("#btnUpdate").click(function () {
            var vc = new PetController();
            vc.Update();
        });
        $("#btnDelete").click(function () {
            var vc = new PetController();
            vc.Delete();
        });

        this.LoadTable();
        this.loadUserOptions();
    }


    this.ValidateInputs = function () {
        var petName = $("#txtName").val();
        var petDescription = $("#txtDescription").val();
        var petAge = $("#txtAge").val();
        var petPetBreedType = $("#txtPetBreedType").val();
        var petPetAggressiveness = $("#txtPetAggressiveness").val();
        var petUserId = $("#txtUserId").val();
        var petStatus = $("#txtStatus").val();

        if (!petName) {
            Swal.fire("Error", "El nombre de la mascota no puede estar vacío.", "error");
            return false;
        }
        if (!petDescription) {
            Swal.fire("Error", "La descripción de la mascota no puede estar vacía.", "error");
            return false;
        }
        if (!petAge) {
            Swal.fire("Error", "La edad de la mascota no puede estar vacía.", "error");
            return false;
        }
        if (!petPetBreedType) {
            Swal.fire("Error", "La raza de la mascota no puede estar vacío.", "error");
            return false;
        }
        if (!petPetAggressiveness) {
            Swal.fire("Error", "La agresividad de la mascota no puede estar vacía.", "error");
            return false;
        }
        if (!petUserId) {
            Swal.fire("Error", "El Usuario no puede estar vacío.", "error");
            return false;
        }
        if (!petStatus) {
            Swal.fire("Error", "El estado del paquete no puede estar vacío.", "error");
            return false;
        }

        return true;
    }

    this.Create = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var pet = {};
        pet.PetName = $("#txtName").val();
        pet.Description = $("#txtDescription").val();
        pet.PetAge = $("#txtAge").val();
        pet.PetBreedType = $("#txtPetBreedType").val();
        pet.PetAggressiveness = $("#txtPetAggressiveness").val();
        pet.CreatedDate = new Date();
        pet.ModifiedDate = new Date();
        pet.UserId = $("#txtUserId").val();
        pet.Status = $("#txtStatus").val();


        var ctrlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Create";

        function successCallback(response) {
            Swal.fire("success!", "Mascota creado correctamente!", "success");
        }

        function failCallback(response, status) {
            Swal.fire("error!",response, "error");
            console.log("fail callback");
        }


        ctrlActions.PostToAPI(serviceRoute, pet, successCallback, failCallback);
    }

    this.Update = function () {
        if (!this.ValidateInputs()) {
            return;
        }
        var pet = {};
        pet.Id = $("#txtPetId").val();
        pet.PetName = $("#txtName").val();
        pet.Description = $("#txtDescription").val();
        pet.PetAge = $("#txtAge").val();
        pet.PetBreedType = $("#txtPetBreedType").val();
        pet.PetAggressiveness = $("#txtPetAggressiveness").val();
        pet.CreatedDate = new Date();
        pet.ModifiedDate = new Date();
        pet.UserId = $("#txtUserId").val();
        pet.Status = $("#txtStatus").val();

        var controlActions = new ControlActions();
        var serviceRoute = this.ApiService + "/Update";

        function successCallback(response) {
            Swal.fire("success!", "Paquete modificadp correctamente!", "success");
        }

        function failCallback(response, status) {
            Swal.fire("error!", response, "error");
            console.log("fail callback");
        }

        controlActions.PutToAPI(serviceRoute, pet, successCallback, failCallback);

    }
    this.Delete = function () {
        var pet = {};
        pet.Id = $("#txtPetId").val();

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

        controlActions.DeleteToAPI(serviceRoute, pet, successCallback, failCallback);
    }
    this.LoadTable = function () {
        var ctrlActions = new ControlActions();
        var urlService = ctrlActions.GetUrlApiService(this.ApiService + "/RetrieveAll")

        var columns = []
        columns[0] = { 'data': 'id' }
        columns[1] = { 'data': 'petName' }
        columns[2] = { 'data': 'description' }
        columns[3] = { 'data': 'petAge' }
        columns[4] = { 'data': 'petBreedType' }
        columns[5] = { 'data': 'petAggressiveness' }
        columns[6] = { 'data': 'createdDate' }
        columns[7] = { 'data': 'modifiedDate' }
        columns[8] = { 'data': 'userId' }
        columns[9] = { 'data': 'status' }


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
                        text: user.IdentificationValue
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