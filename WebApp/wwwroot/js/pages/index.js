function IndexController() {
    this.title = "Inicio";

    this.InitView = function () {
        document.title = this.title;

        this.LoadPackages();
    }

    this.ServicesRetrieveAll = async function (services) {        
        // List of promises
        var listPromises = [];

        for (var service in services) {
            let controlActions = new ControlActions();
            let serviceRoute = "Service/RetrieveById?id=" + services[service];
            
            // create a promise for each service
            var promise = new Promise((resolve, reject) => {             
                controlActions.GetToApi(serviceRoute, resolve, reject);
            });

            // add the promise to the list
            listPromises.push(promise);
        }

        // after all promises are resolved, return the services
        return Promise.all(listPromises);
    }

    this.LoadPackages = function () {
        let controlActions = new ControlActions();
        let serviceRoute = "Package/RetrieveAll";

        function successCallback(response) {
            // filter packages with status = 1
            let packages = response.filter(p => p.status == 1);
            
            // for each package create a card
            packages.forEach(async package => {
                const vc = new IndexController();
                const services = await vc.ServicesRetrieveAll(package.services);
                const cost = services.reduce((acc, s) => acc + s.serviceCost, 0);

                let html =`
                <div class="packages-card" >
                    <h5>${package.packageName}</h5>
                    <p class="packages-card-description">${package.description}</p>
                    <hr />
                    <h6 class="packages-card-service-title">Servicios incluidos</h6>
                    <div class="packages-card-items">
                        ${services.map(s => `<p>${s.serviceName}</p>`).join('')}
                    </div>
                    <hr />
                    <h6 class="package-card-restrictions">Aplica para mascotas con las siguientes características</h6>
                    <div class="packages-card-items-restrictions">
                        <p>Raza: ${package.petBreedType}</p>
                        <p>Tamaño: ${package.petSize}</p>
                        <p>Agresividad: ${package.petAggressiveness}</p>
                    </div>
                    <hr/>
                    <p class="packages-card-price">Precio: ₡${cost}</p>
                    <button class="btn btn-primary" id-package="${package.packageId}">Reservar</button>
                </div>`;

                $('.packages-section').append(html);
            });
        };

        function failCallback(response) {
            console.log(response);
        }

        controlActions.GetToApi(serviceRoute, successCallback, failCallback);
    };
};

$(document).ready(function () {
    const viewCont = new IndexController();
    viewCont.InitView();
});