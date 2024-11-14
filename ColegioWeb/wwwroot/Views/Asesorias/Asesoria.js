let tablaData;
let idEditar = 0;
const controlador = "Asesorias";
const modal = "mdData";
const preguntaEliminar = "¿Desea cancelar su sesión?";
const confirmaEliminar = "Su asesoria fue cancelada.";

document.addEventListener("DOMContentLoaded", function (event) {

    tablaData = $('#tbData').DataTable({
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `/${controlador}/ListaAsesoriasPendiente`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Fecha Asesoria", "data": "fechaAsesoria", width: "150px" },
            { title: "Hora Asesoria", "data": "horaAsesoria", width: "150px" },
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
                    return `<button type="button" class="btn btn-sm btn-outline-danger me-1 btn-cancelar">Cancelar</button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});


$("#tbData tbody").on("click", ".btn-cancelar", function () {
    let filaSeleccionada = $(this).closest('tr');
    let data = tablaData.row(filaSeleccionada).data();

    Swal.fire({
        text: `${preguntaEliminar}`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, continuar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {

            fetch(`/${controlador}/Cancelar?Id=${data.idAsesoria}`, {
                method: "DELETE",
                headers: { 'Content-Type': 'application/json;charset=utf-8' }
            }).then(response => {
                return response.ok ? response.json() : Promise.reject(response);
            }).then(responseJson => {
                if (responseJson.data == "") {
                    Swal.fire({
                        title: "¡Listo!",
                        text: confirmaEliminar,
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error",
                        text: "No se pudo cancelar.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error",
                    text: "No se pudo cancelar.",
                    icon: "warning"
                });
            })
        }
    });
})
