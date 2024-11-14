let tablaData;
let tablaData2;
let idAsesoriaSeleccionada = 0;
const controlador = "Docente";
const modal = "mdData";
const preguntaEliminar = "¿Desea cancelar su asesoria?";
const confirmaEliminar = "Su asesoria fue cancelada.";

document.addEventListener("DOMContentLoaded", function (event) {

    tablaData = $('#tbData').DataTable({
        processing:true,
        responsive: true,
        scrollX: true,
        "ajax": {
            "url": `/${controlador}/ListaAsesoriasAsignadas?IdEstadoAsesoria=1`,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { title: "Fecha Asesoria", "data": "fechaAsesoria", width: "150px" },
            { title: "Hora Asesoria", "data": "horaAsesoria", width: "150px" },
            {
                title: "Alumno", "data": "usuario", render: function (data, type, row) {
                    return `${data.nombre} ${data.apellido}`
                }
            },
            {
                title: "Estado", "data": "estadoAsesoria", render: function (data, type, row) {
                    return data.nombre == "Pendiente" ? `<span class="badge bg-primary">${data.nombre}</span>` : 
                        `<span class="badge bg-success">${data.nombre}</span>`
                }
            },
            {
                title: "", "data": "idAsesoria", width: "100px", render: function (data, type, row) {
                    return `<button type="button"  class="btn btn-sm btn-outline-danger me-1 btn-cancelar"  >Cancelar</button>`
                }
            },
            {
                title: "", "data": "idAsesoria", width: "100px", render: function (data, type, row) {
                    return `<button type="button" class="btn btn-sm btn-outline-warning me-1 btn-indicaciones">Indicaciones</button>`
                }
            }
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});







$("#cboEstadoAsesoria").on("change", function () {
    var nueva_url = `/${controlador}/ListaAsesoriasAsignadas?IdEstadoAsesoria=${$("#cboEstadoAsesoria").val()}`
   
        tablaData.ajax.url(nueva_url).load();

   

   
})

$("#tbData tbody").on("click", ".btn-indicaciones", function () {
    const filaSeleccionada = $(this).closest('tr');
    const data = tablaData.row(filaSeleccionada).data();
    console.log(data)
    idCitaSeleccionada = data.idAsesoria;
    $("#txtIndicaciones").val(data.indicaciones)
    $(`#${modal}`).modal('show');
    $("#txtIndicaciones").trigger("focus");

    if (data.estadoAsesoria.nombre == "Atendido") {
        $("#txtIndicaciones").prop('disabled', true);
        $("#btnTerminarAsesoria").prop('disabled', true);
        $(".btn-cancelar").prop('disabled', true);
       

        $('.alert-primary').hide();
    } else if (data.estadoAsesoria.nombre == "Anulado") {

        $("#txtIndicaciones").prop('disabled', true);
        $("#btnTerminarAsesoria").prop('disabled', true);
        $(".btn-cancelar").prop('disabled', true);

        $('.alert-primary').hide();
    }
        else
    {
        $("#txtIndicaciones").prop('disabled', false);
        $("#btnTerminarAsesoria").prop('disabled', false);
        $('.alert-primary').show();
    }
})

$("#btnTerminarAsesoria").on("click", function () {
    if ($("#txtIndicaciones").val().trim() == ""
    ) {
        Swal.fire({
            title: "Importante!",
            text: "Debe ingresar las indicaciones.",
            icon: "warning"
        });
        return
    }

    const objeto = {
        IdAsesoria: idAsesoriaSeleccionada,
        EstadoAsesoria: {
            IdEstadoAsesoria: 2
        },
        Indicaciones: $("#txtIndicaciones").val().trim(),
    }

    fetch(`/${controlador}/CambiarEstado`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data == "") {
            Swal.fire({
                title: "Listo!",
                text: "La asesoria fue marcada como ATENDIDA",
                icon: "success"
            });
            $(`#${modal}`).modal('hide');
            tablaData.ajax.reload();
        } else {
            Swal.fire({
                title: "Error!",
                text: responseJson.data,
                icon: "warning"
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: "Error!",
            text: "No se pudo registrar.",
            icon: "warning"
        });
    })

})


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
                        title: "Listo!",
                        text: confirmaEliminar,
                        icon: "success"
                    });
                    tablaData.ajax.reload();
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: "No se pudo cancelar.",
                        icon: "warning"
                    });
                }
            }).catch((error) => {
                Swal.fire({
                    title: "Error!",
                    text: "No se pudo cancelar.",
                    icon: "warning"
                });
            })
        }
    });
})
