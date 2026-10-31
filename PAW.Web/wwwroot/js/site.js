// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    $(function () {
    $(document).on("click", ".btn-crud", function () {
        const url = $(this).data("url");

        if (!url) {
            console.error("El botón no tiene definido el atributo data-url.");
            return;
        }

        $.get(url)
            .done(function (html) {
                $("#modalBodyContainer").html(html);

                if ($.validator && $.validator.unobtrusive) {
                        $.validator.unobtrusive.parse("#modalBodyContainer");
                 }

                const modalElement = document.getElementById("crudModal");
                const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

                modal.show();
            })
            .fail(function (xhr) {
                console.error(xhr.responseText);
                alert("Error al cargar la información del producto.");
            });
    });

    $(document).on("click", ".btn-switch-to-edit", function () {
        const url = $(this).data("url");

        $.get(url)
            .done(function (html) {
                $("#modalBodyContainer").html(html);
            })
            .fail(function (xhr) {
                console.error(xhr.responseText);
                alert("Error al cargar la edición del producto.");
            });
    });
});


//Clase #5
async function getExchangeRate() {

    const response = await fetch("/exchange-rate");

    if (!response.ok) {
        throw new Error(
            `HTTP ${response.status}`
        );
    }

    return await response.json();
}


async function loadExchangeRate() {

    const status =
        document.getElementById(
            "exchange-rate-status"
        );

    try {

        const exchangeRate =
            await getExchangeRate();

        document.getElementById(
            "dollar-buy"
        ).innerText =
            exchangeRate.dolar.compra.valor;

        document.getElementById(
            "dollar-sell"
        ).innerText =
            exchangeRate.dolar.venta.valor;

        document.getElementById(
            "euro-colones"
        ).innerText =
            exchangeRate.euro.colones;

        status.innerText = "";

    }
    catch (error) {

        console.error(error);

        status.innerText =
            "Tipo de cambio no disponible";
    }
}


document.addEventListener(
    "DOMContentLoaded",
    loadExchangeRate
);