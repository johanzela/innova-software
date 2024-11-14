using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColegioWeb.Controllers
{
    public class DocenteHorarioController : Controller
    {
        private readonly IDocenteRepositorio _repositorio;
        public DocenteHorarioController(IDocenteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DocenteHorario> lista = await _repositorio.ListaDocenteHorario();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }


        public async Task<IActionResult> Guardar([FromBody] DocenteHorario objeto)
        {
            string respuesta = await _repositorio.RegistrarHorario(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int Id)
        {
            string respuesta = await _repositorio.EliminarHorario(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

    }
}
