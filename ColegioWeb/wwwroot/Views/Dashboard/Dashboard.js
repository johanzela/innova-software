$(document).ready(function () {

    fetch(`/Home/Dashboard`, {
        method: "GET",
        headers: { 'Content-Type': 'application/json;charset=utf-8' }
    }).then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    }).then(responseJson => {
        const data = responseJson.data;
        $("#hDocente").text(data.totalDocentes);
        $("#hCurso").text(data.totalCurso);
        $("#hAsesoriaPendiente").text(data.totalAsesoriaPendiente);
        $("#hAsesoriaAtendida").text(data.totalAsesoriaAtendida);
        $("#hAsesoriaAnulada").text(data.totalAsesoriaAnulada);
    }).catch((error) => {
        Swal.fire({
            title: "¡Error!",
            text: "No se encontraron coincidencias.",
            icon: "warning"
        });
    })
})