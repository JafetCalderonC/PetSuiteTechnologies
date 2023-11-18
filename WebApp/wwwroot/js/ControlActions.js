
function ControlActions() {
    //Ruta base del API
    this.URL_API = "https://localhost:7299/api/";
    this.GetUrlApiService = function (service) {
        return this.URL_API + service;
    }

    this.GetTableColumsDataName = function (tableId) {
        var val = $('#' + tableId).attr("ColumnsDataName");

        return val;
    }

    this.FillTable = function (service, tableId, refresh) {

        if (!refresh) {
            columns = this.GetTableColumsDataName(tableId).split(',');
            var arrayColumnsData = [];


            $.each(columns, function (index, value) {
                var obj = {};
                obj.data = value;
                arrayColumnsData.push(obj);
            });
            //Esto es la inicializacion de la tabla de data tables segun la documentacion de 
            // datatables.net, carga la data usando un request async al API
            $('#' + tableId).DataTable({
                "processing": true,
                "ajax": {
                    "url": this.GetUrlApiService(service),
                    "dataSrc": 'Data',
                    "beforeSend": function (request) {
                        request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
                    }
                },
                "columns": arrayColumnsData
            });
        } else {
            //RECARGA LA TABLA
            $('#' + tableId).DataTable().ajax.reload();
        }

    }

    this.GetSelectedRow = function () {
        var data = sessionStorage.getItem(tableId + '_selected');

        return data;
    };

    this.BindFields = function (formId, data) {
        console.log(data);
        $('#' + formId + ' *').filter(':input').each(function (input) {
            var columnDataName = $(this).attr("ColumnDataName");
            this.value = data[columnDataName];
        });
    }

    this.GetDataForm = function (formId) {
        var data = {};

        $('#' + formId + ' *').filter(':input').each(function (input) {
            var columnDataName = $(this).attr("ColumnDataName");
            data[columnDataName] = this.value;
        });

        console.log(data);
        return data;
    }


    /* ACCIONES VIA AJAX, O ACCIONES ASINCRONAS*/

    this.PostToAPI = function (service, data, successCallback, failCallBack) {
        $.ajax({
            type: "POST",
            url: this.GetUrlApiService(service),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                if (successCallback) {
                    successCallback(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var response = jqXHR.responseJSON || jqXHR.responseText;

                // if 200 ok
                if (jqXHR.status == 200) {
                    if (successCallback) {
                        successCallback();
                    }
                    return;
                }

                if (failCallBack) {
                    failCallBack(response, jqXHR.status);
                }
            }
        });
    };

    this.PutToAPI = function (service, data, successCallback, failCallBack) {
        $.ajax({
            type: "PUT",
            url: this.GetUrlApiService(service),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                if (successCallback) {
                    successCallback(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var response = jqXHR.responseJSON || jqXHR.responseText;

                // if 200 ok
                if (jqXHR.status == 200) {
                    if (successCallback) {
                        successCallback();
                    }
                    return;
                }

                if (failCallBack) {
                    failCallBack(response, jqXHR.status);
                }
            }
        });
    };

    this.DeleteToAPI = function (service, data, successCallback, failCallBack) {
        $.ajax({
            type: "DELETE",
            url: this.GetUrlApiService(service),
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                if (successCallback) {
                    successCallback(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var response = jqXHR.responseJSON || jqXHR.responseText;

                // if 200 ok
                if (jqXHR.status == 200) {
                    if (successCallback) {
                        successCallback();
                    }
                    return;
                }

                if (failCallBack) {
                    failCallBack(response, jqXHR.status);
                }
            }
        });
    };

    this.GetToApi = function (service, successCallback, failCallBack) {
        const url = this.GetUrlApiService(service);
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem('token'));
            },
            success: function (data) {
                if (successCallback) {
                    successCallback(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var response = jqXHR.responseJSON || jqXHR.responseText;

                // if 200 ok
                if (jqXHR.status == 200) {
                    if (successCallback) {
                        successCallback();
                    }
                    return;
                }

                if (failCallBack) {
                    failCallBack(response, jqXHR.status);
                }
            }
        });
    };
}