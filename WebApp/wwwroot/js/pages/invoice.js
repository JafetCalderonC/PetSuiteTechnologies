let idInvoice = 0;
let invoices = [];

function RetrieveAllInvoice() {
    function successCallback(response) {


        invoicesOptionsResponse = response;
        invoicesOptionsResponse.forEach(function (obj) {
            var newObj = { id: obj.id, totalAmount: obj.totalAmount, discountAmount: obj.discountAmount, status: obj.status, reservationId: obj.reservationId  };
            invoices.push(newObj);
        });
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
    const serviceRoute = "Invoice" + "/RetrieveAll";
    controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
}

//search invoice amount by id
function SearchInvoiceAmount(id) {
    let invoiceAmount = 0;
    invoices.forEach(function (obj) {
        if (obj.id == id) {
            invoiceAmount = obj.totalAmount;
        }
    });
    return invoiceAmount;
}

//search invoice by reservationId
function SearchInvoiceByReservationId(reservationId) {
    let invoice = null;
    invoices.forEach(function (obj) {
        if (obj.reservationId == reservationId) {
            invoice = obj;
        }
    });
    return invoice;
}
function CreateInvoiceMapper(userId, reservationId, totalAmount) {
    let invoiceData = { id };
    invoiceData.invoiceNumber = null;
    invoiceData.issueDate = new Date();
    let dueDate = new Date(invoiceData.issueDate);
    dueDate.setDate(issueDate.getDate() + 10);
    invoiceData.userID = userId;
    invoiceData.reservationId = reservationId;
    invoiceData.totalAmount = totalAmount;
    invoiceData.status = 1;
    invoiceData.taxAmount = 0.10;
    invoiceData.discountCode = null;
    invoiceData.discountAmount = null;
    return invoiceData;

};

function UpdateInvoiceMapper(id, status, discountAmount, discountCode) {// descuento = 2 y pagado = 0 // pendiente = 1

let invoiceData = { id };
    invoiceData.status = status;
    invoiceData.discountAmount = discountAmount;
    if (discountCode != null) {
        invoiceData.discountCode = discountCode;
    }
    return invoiceData;
}


 function InvoiceController() {
    this.ApiInvoice = "Invoice";
    this.title = "Factura";

    this.InitView = function () {

        $(document).on('click', '.btnPagar', function () {
            const vc = new InvoiceController();
            vc.Pagar($(this).data('id'), 0);
        });
        RetrieveAllInvoice();
        this.LoadTable();

    }
    this.Create = async function (userId, reservationId, totalAmount) {
        let data = await CreateInvoiceMapper(userId, reservationId, totalAmount);
        function successCallback(response) {
            Swal.fire({
                icon: 'success',
                title: 'Se ha creado una factura',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);

        }
        const controlActions = new ControlActions();
        const serviceRoute = this.ApiInvoice + "/Create";
        controlActions.PostToAPI(serviceRoute, data, successCallback, failCallBack);
    }

    this.Pagar = async function (id,status,discountAmount) {
        let data = await UpdateInvoiceMapper(id, status, discountAmount);
        function successCallback(response) {
            Swal.fire({
                icon: 'success',
                title: 'Se ha actualizado el estado de la factura',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);

        }
        const controlActions = new ControlActions();
        const serviceRoute = this.ApiInvoice + "/Update";
        controlActions.PostToAPI(serviceRoute, data, successCallback, failCallBack);
    }
    this.BorrarConDescuento = async function (ReservationId) {
        var invoiceId = SearchInvoiceByReservationId(ReservationId);
        var status = 2;
        var discountAmountToChange = SearchInvoiceAmount(invoiceId);
        let data = await UpdateInvoiceMapper(invoiceId, status, discountAmountToChange);
        function successCallback(response) {
            Swal.fire({
                icon: 'success',
                title: 'Se ha actualizado el estado de la factura',
                footer: 'PetSuite Technologies',
                confirmButtonText: 'Entendido'
            });
        }

        function failCallBack(response) {
            showValidationErrors(response);

        }
        const controlActions = new ControlActions();
        const serviceRoute = this.ApiInvoice + "/Update";
        controlActions.PostToAPI(serviceRoute, data, successCallback, failCallBack);
    }

    this.LoadTable = function () {
        let controlActions = new ControlActions();
        let serviceRoute = controlActions.GetUrlApiService(this.ApiInvoice + "/RetrieveAll");

        let columns = [];
        columns[0] = { "data": "issueDate", title: "Fecha de emision" };
        columns[1] = { "data": "dueDate", title: "Fecha de vencimiento" };
        columns[2] = { "data": "userId", title: "Usuario" };
        columns[3] = { "data": "reservationId", title: "Reservacion" };
        columns[4] = { "data": "totalAmount", title: "Monto total" };
        columns[5] = { "data": "status", title: "Estado" };
        columns[6] = { "data": "taxAmount", title: "Monto de impuesto" };
        columns[7] = { "data": "discountAmount", title: "Monto de descuento" };
        columns[8] = {
            "orderable": false,
            'searchable': false,
            "title": "Acciones",
            "data": "id",
            "render": function (value) {
                return '<div style="display: flex;">' +
                    '<button class="btnPagar btn btn-danger" data-id="' + value + '">Pagar</button>' +
                    '</div>';
            }
        };

        $('#tblListInvoice').DataTable({
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

    }


}
$(document).ready(function () {
    var viewCont = new InvoiceController();
    viewCont.InitView();
});




//{

//this.LoadTable = function () {
//    let controlActions = new ControlActions();
//    let serviceRoute = controlActions.GetUrlApiService(this.ApiService + "/RetrieveAll");

//    let columns = [];
//    columns[0] = { "data": "serviceName", title: "Nombre" };
//    columns[1] = { "data": "serviceDescription", title: "Descripcion" };
//    columns[2] = { "data": "serviceStatus", title: "Estado" };
//    columns[3] = { "data": "serviceCost", title: "Costo" };
//    columns[4] = {
//        "data": "serviceCreatedDate",
//        "title": "Fecha de creacion",
//        "render": function (value) {
//            return formatDateTime(new Date(value));
//        }
//    };
//    columns[5] = {
//        "data": "serviceModifiedDate",
//        "title": "Fecha de modificacion",
//        "render": function (value) {
//            return formatDateTime(new Date(value));
//        }
//    };
//    columns[6] = {
//        "orderable": false,
//        'searchable': false,
//        "title": "Acciones",
//        "data": "id",
//        "render": function (value) {
//            return '<div style="display: flex;">' +
//                '<button class="btnEdit btn btn-primary m-3 mt-0 mb-0" data-id="' + value + '" >Editar</button>' +
//                '<button class="btnDelete btn btn-danger" data-id="' + value + '">Eliminar</button>' +
//                '</div>';
//        }
//    };

//    $('#tblListServices').DataTable({
//        "responsive": true,
//        "processing": true,
//        "ajax": {
//            "url": serviceRoute,
//            "dataSrc": "",
//            "beforeSend": function (request) {
//                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
//            }
//        },
//        "columns": columns,
//        "language": {
//            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
//        },
//    });
//};
//    "id": 0,
//        "invoiceNumber": "string",
//            "issueDate": "2023-12-08T01:20:27.253Z",
//                "dueDate": "2023-12-08T01:20:27.253Z",
//                    "userId": 0,
//                        "reservationId": 0,
//                            "totalAmount": 0,
//                                "status": 0,
//                                    "taxAmount": 0,
//                                        "discountCode": "string",
//                                            "discountAmount": 0