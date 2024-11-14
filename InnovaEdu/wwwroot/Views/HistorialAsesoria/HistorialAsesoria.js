let tablaData;
const controlador = "HistorialAsesorias";
const modal = "mdData";
const preguntaEliminar = "Desea cancelar su sesión?";
const confirmaEliminar = "Su asesoría fue cancelada.";

document.addEventListener("DOMContentLoaded", function (event) {

    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `/${controlador}/ListaHistorialAsesorias`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Fecha Asesoria", "data": "fechaAsesoria", width: "150px" },
            {
                title: "Curso", "data": "curso", render: function (data, type, row) {
                    return data.nombre
                }
            },
            {
                title: "Docente", "data": "docente", render: function (data, type, row) {
                    return `${data.nombres} ${data.apellidos}`
                }
            },
            {
                title: "", "data": "idAsesoria", width: "100px", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-primary me-1 btn-indicaciones">Indicaciones</button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

$("#tbData tbody").on("click", ".btn-indicaciones", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();
    $("#txtIndicaciones").val(data.indicaciones)
    $(`#${modal}`).modal('show');
    $("#txtIndicaciones").prop('disabled', true);
})