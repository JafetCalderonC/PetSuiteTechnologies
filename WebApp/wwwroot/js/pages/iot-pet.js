const BLUE = "#32bbad";
let temperaturaChart, humedadChart, movimientoChart;

function GetData(key, response) {
    try {
        const pets = response;
        return pets.map(pet => ({
            t: pet.created,
            y: pet[key]
        }));

    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: error,
            footer: 'PetSuite Technologies',
            confirmButtonText: 'Entendido'
        });
        return [];
    }
};

function chartOptions(title) {
    return {
        scales: {
            xAxes: [
                {
                    type: "time",
                    distribution: "series",
                    gridLines: {
                        drawOnChartArea: false,
                    },
                    ticks: {
                        source: "data",
                        autoSkip: true,
                    },
                },
            ],
        },
        legend: {
            display: true,
            position: "top",
        },
        title: {
            display: true,
            text: title,
        },
    };
};

function IoTPetController() {
    this.ApiService = "IoTPet";
    this.title = "Iot de mascotas";

    this.InitView = function () {
        document.Title = this.Title;


        setInterval(() => {
            const vc = new IoTPetController();
            vc.temperatura();
        }, 30000);

        setInterval(() => {
            const vc = new IoTPetController();
            vc.humedad();
        }, 30000);

        setInterval(() => {
            const vc = new IoTPetController();
            vc.movimiento();
        }, 30000);

        const vc = new IoTPetController();
        vc.temperatura();
        vc.humedad();
        vc.movimiento();
    }

    this.temperatura = async function () {
        function successCallback(response) {
            const idCanvas = "chart-temperatura";
            const data = GetData("temperature", response)

            const ctx = document.getElementById(idCanvas).getContext("2d");
            if (!window.temperaturaChart) {
                window.temperaturaChart = new Chart(ctx, {
                    type: "line",
                    data: {
                        datasets: [
                            {
                                label: "Temperatura",
                                data,
                                borderColor: "BLUE",
                                fill: false,
                            },
                        ],
                    },
                    options: chartOptions("Gráfico de Temperatura")
                });
            } else {
                window.temperaturaChart.data.datasets[0].data = data;
                window.temperaturaChart.update();
            }
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

        let controlActions = new ControlActions();
        let serviceRoute = this.ApiService + "/RetrieveAll";
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }

    this.humedad = async function () {
        function successCallback(response) {
            const idCanvas = "chart-humedad";
            const data = GetData("humidity", response)

            const ctx = document.getElementById(idCanvas).getContext("2d");
            if (!window.humedadChart) {
                window.humedadChart = new Chart(ctx, {
                    type: "line",
                    data: {
                        datasets: [
                            {
                                label: "Humedad",
                                data,
                                borderColor: "BLUE",
                                fill: false,
                            },
                        ],
                    },
                    options: chartOptions("Gráfico de Humedad")
                });
            } else {
                window.humedadChart.data.datasets[0].data = data;
                window.humedadChart.update();
            }
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

        let controlActions = new ControlActions();
        let serviceRoute = this.ApiService + "/RetrieveAll";
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }
    this.movimiento = async function () {
        function successCallback(response) {
            const idCanvas = "chart-movimiento";
            const data = GetData("contadorDePasos", response)

            const ctx = document.getElementById(idCanvas).getContext("2d");
            if (!window.movimientoChart) {
                window.movimientoChart = new Chart(ctx, {
                    type: "bar",
                    data: {
                        datasets: [
                            {
                                label: "Movimiento",
                                data,
                                backgroundColor: "rgba(255, 99, 132, 0.6)",
                            },
                        ],
                    },
                    options: chartOptions("Gráfico de Movimientos")
                });
            } else {
                window.movimientoChart.data.datasets[0].data = data;
                window.movimientoChart.update();
            }
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

        let controlActions = new ControlActions();
        let serviceRoute = this.ApiService + "/RetrieveAll";
        controlActions.GetToApi(serviceRoute, successCallback, failCallBack);
    }
}

$(document).ready(function () {
    var viewCont = new IoTPetController();
    viewCont.InitView();
});