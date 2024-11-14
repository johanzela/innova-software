let Cursos = [];
let Docentes = [];
let DocenteHorario = [];
const modal = "mdData";
let IdCursoSeleccionado = 0;
let IdDocenteSeleccionado = 0;
let IdHoraSeleccionado = 0;
let CursoSeleccionado = "";
let DocenteSeleccionado = "";
let HoraSeleccionado = "";

const IndexTabs = [0,1,2]


document.addEventListener("DOMContentLoaded", function (event) {
    $.datepicker.setDefaults($.datepicker.regional['es']);
    $("#tabs").tabs();
    $("#txtBuscarCurso").trigger("focus");
    


    $("#tab-curso").LoadingOverlay("show");
    fetch(`/Curso/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        $("#tab-curso").LoadingOverlay("hide");
        if (responseJson.data.length > 0) {
            Cursos = responseJson.data;
            BuscarCurso("");
        }
    }).catch((error) => {
        $("#tab-curso").LoadingOverlay("hide");
        console.log(error)
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })

 
    $("#tabs").tabs("option", "disabled", [1, 2]);
    $("#btnSiguiente").prop("disabled", true);
    
});

$("#txtBuscarCurso").on("input", function () {

    IdCursoSeleccionado = 0;
    BuscarCurso($(this).val());
    $("#btnSiguiente").prop("disabled", true);
});
$("#txtBuscarDocente").on("input", function () {

    IdDocenteSeleccionado = 0;
    BuscarDocente($(this).val());
    $("#btnSiguiente").prop("disabled", true);
});


$(document).on("click", "div.card-curso", function () {

    $(".card-curso").removeClass("text-white bg-primary");
    $(this).addClass("text-white bg-primary");
    IdCursoSeleccionado = $(this).data("id");
    CursoSeleccionado = $(this).data("text");
    $("#btnSiguiente").prop("disabled", false);
});

$(document).on("click", "div.card-docente", function () {

    $(".card-docente").removeClass("text-white bg-primary");
    $(this).addClass("text-white bg-primary");
    IdDocenteSeleccionado = $(this).data("id");
    DocenteSeleccionado = $(this).data("text");
    $("#btnSiguiente").prop("disabled", false);
});

$(document).on("click", "div.card-hora", function () {

    $(".card-hora").removeClass("text-white bg-primary");
    $(this).addClass("text-white bg-primary");
    IdHoraSeleccionado = $(this).data("id");
    HoraSeleccionado = $(this).data("text");

    if ($("#txtFechaAsesoria").val() != "" && IdHoraSeleccionado != 0)
        $("#btnSiguiente").prop("disabled", false);
});


function BuscarCurso(valor) {
    $("#contenedor-cursos").html("")
    const resultadosFiltrados = Cursos.filter(element => element.nombre.toLowerCase().includes(valor.toLowerCase()));
    const resumenFiltro = resultadosFiltrados.slice(0, 18);

    resumenFiltro.forEach(function (item) {
        $("#contenedor-cursos").append(
            `<div class="col mb-4">
                <div class="card h-100 card-curso" style="cursor: pointer" data-id="${item.idCursos}" data-text="${item.nombre}">
                    <div class="card-body">
                        <h5 class="card-title"><i class="fa-solid fa-clipboard"></i> ${item.nombre}</h5>
                    </div>
                </div>
            </div>`
        )
    })
}

function BuscarDocente(valor) {
    $("#contenedor-docente").html("")
    const resultadosFiltrados = Docente.filter(element => (element.nombres + element.apellidos).toLowerCase().includes(valor.toLowerCase()));
    resultadosFiltrados.forEach(function (item) {
        const acronimo = item.genero == "F" ? "Profesora.":"Profesor."
        $("#contenedor-docente").append(
            `<div class="col mb-4">
                <div class="card h-100 card-docente" style="cursor: pointer" data-id="${item.idDocente}" data-text="${acronimo} ${item.nombres} ${item.apellidos}">
                    <div class="card-body">
                        <h5 class="card-title"><i class="fa-solid fa-user-doctor"></i> ${acronimo} ${item.nombres} ${item.apellidos}</h5>
                    </div>
                </div>
            </div>`
        )
    })
}


$("#btnSiguiente").on("click", function () {
    const indexTab = $("ul li.ui-state-active").index();

    let habilitarIndex = indexTab;
    if (indexTab + 1 <= 2) {

        habilitarIndex = indexTab + 1;
        bloquearIndex = IndexTabs.filter((i) => i != habilitarIndex);
        $("#tabs").tabs("option", "disabled", bloquearIndex);
        $("#tabs").tabs({ active: habilitarIndex });
        $("#btnSiguiente").prop("disabled", true);

        if (habilitarIndex == 1) {
            $("#txtBuscarDocente").val("");
            $("#txtBuscarDocente").trigger("focus");
            ObtenerDocentes()
        } else if (habilitarIndex == 2) {
            
            $("#txtFechaAsesoria").val("");
            ObtenerDocenteHorarioDetalle()

           
        }
    }
    if (indexTab == 2) {
        $("#txtCurso").val(CursoSeleccionado);
        $("#txtDocente").val(DocenteSeleccionado);
        $("#txtFechadeAsesoria").val($("#txtFechaAsesoria").val());
        $("#txtHoraAsesoria").val(HoraSeleccionado);
        $("#mdData").modal("show")
    }
    
});

$("#btnRegresar").on("click", function () {

    const indexTab = $("ul li.ui-state-active").index();
    let habilitarIndex = indexTab;
    if (indexTab - 1 >= 0) habilitarIndex = indexTab - 1;
    bloquearIndex = IndexTabs.filter((i) => i != habilitarIndex);
    $("#tabs").tabs("option", "disabled", bloquearIndex);
    $("#tabs").tabs({ active: habilitarIndex });
    $("#btnSiguiente").prop("disabled", true);

    if (habilitarIndex == 0) {
        $("#txtBuscarCurso").val("");
        $("#txtBuscarCurso").trigger("focus");
        $(".card-curso").removeClass("text-white bg-primary");
        BuscarCurso("");
    } else if (habilitarIndex == 1) {
        $("#txtBuscarDocente").val("");
        $("#txtBuscarDocente").trigger("focus");
        $(".card-docente").removeClass("text-white bg-primary");
        BuscarDocente("");
    }
});


function ObtenerDocentes() {
    $("#tab-docente").LoadingOverlay("show");

    fetch(`/Docente/Lista`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        if (responseJson.data.length > 0) {
            Docentes = responseJson.data.filter(element => element.curso.idCursos == IdCursoSeleccionado);
            BuscarDocente("")
        }
        $("#tab-docente").LoadingOverlay("hide");
    }).catch((error) => {
        $("#tab-docente").LoadingOverlay("hide");
        console.log(error)
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })

}

function ObtenerDocentesHorarioDetalle() {
    $("#txtFechaAsesoria").datepicker("destroy");
    $("#tab-horario").LoadingOverlay("show");
    $("#contenedor-am").html("");
    $("#contenedor-pm").html("");

    fetch(`/Asesorias/ListaDocenteHorarioDetalle?Id=${IdDocenteSeleccionado}`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
      
        if (responseJson.data.length > 0) {
            const arraySoloFechas = responseJson.data.map(item => item.fecha);
            DocenteHorario = responseJson.data

            $("#txtFechaAsesoria").datepicker({
                defaultDate: "",
                minDate: 0,
                beforeShowDay: function (date) {
                    // Formatear la fecha actual al formato "YYYY-MM-DD"
                    var formattedDate = $.datepicker.formatDate("dd/mm/yy", date);
                    // Verificar si la fecha está en el array de fechas permitidas
                    var esFechaPermitida = ($.inArray(formattedDate, arraySoloFechas) !== -1);
                    // Habilitar o deshabilitar la fecha en el calendario
                    return [esFechaPermitida, ""];
                },
                onSelect: function (dateText, inst) {
                    $("#btnSiguiente").prop("disabled", true);
                    $("#contenedor-am").html("");
                    $("#contenedor-pm").html("");

                    const selectedDate = $(this).val();
                    
                    const Fecha = DocenteHorario.find(element => element.fecha == selectedDate);
  
                    const HorarioAM = Fecha.horarioDTO.filter(element => element.turno == "AM");
                    const HorarioPM = Fecha.horarioDTO.filter(element => element.turno == "PM");
                    HorarioAM.forEach(function (item) {
                        $("#contenedor-am").append(
                            `<div class="col mb-4" >
                                <div class="text-center card-hora" style="cursor: pointer;border-radius: 0.375rem;border: 1px solid #ccc !important;" data-id="${item.idDocenteHorarioDetalle}" data-text="${item.turnoHora}">
                                    <h6 class="card-title mt-2">${item.turnoHora}</h6>
                                </div>
                             </div>`
                        )
                    })
                    HorarioPM.forEach(function (item) {
                        $("#contenedor-pm").append(
                            `<div class="col mb-4" >
                                <div class="text-center card-hora" style="cursor: pointer;border-radius: 0.375rem;border: 1px solid #ccc !important;" data-id="${item.idDocenteHorarioDetalle}" data-text="${item.turnoHora}">
                                    <h6 class="card-title mt-2">${item.turnoHora}</h6>
                                </div>
                             </div>`
                        )
                    })
                }
            });

          
        } else {
            Swal.fire({
                text: "No hay horarios disponibles.",
                icon: "warning"
            });
        }
        $("#tab-horario").LoadingOverlay("hide");
    }).catch((error) => {
        $("#tab-horario").LoadingOverlay("hide");
        Swal.fire({
            title: "Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })
}

$("#btnAgendar").on("click", function () {

    if ($("#txtFechaAsesoria").val() == "" && IdHoraSeleccionado == 0) {
        Swal.fire({
            title: "Error!",
            text: "Falta completar datos.",
            icon: "warning"
        });
        return
    }
    
    let objeto = {
        DocenteHorarioDetalle: {
            IdDocenteHorarioDetalle: IdHoraSeleccionado
        },
        EstadoAsesoria: {
            IdEstadoAsesoria: 1
        },
        FechaAsesoria: moment($("#txtFechaAsesoria").val(), "DD/MM/YYYY").format('DD/MM/YYYY')
    }

    fetch(`/Asesorias/Guardar`, {
        method: "POST",
        headers: { 'Content-Type': 'application/json;charset=utf-8' },
        body: JSON.stringify(objeto)
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        
        if (responseJson.data == "") {
            Swal.fire({
                title: "Felicidades!",
                text: "Su asesoria fue registrada!",
                icon: "success"
            }).then(function () {
                window.location.href = '/Asesorias/Index'
            });
            $(`#${modal}`).modal('hide');
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