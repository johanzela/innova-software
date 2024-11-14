using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColegioWeb.Controllers
{
    public class DocenteController : Controller
    {
        private readonly IDocenteRepositorio _repositorio;
        private readonly IAsesoriaRepositorio _repositorioAsesoria;
        public DocenteController(IDocenteRepositorio repositorio, IAsesoriaRepositorio repositorioAsesoria)
        {
            _repositorio = repositorio;
            _repositorioAsesoria = repositorioAsesoria;
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Alumno,Docente")]
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Docente> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Docente objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Docente objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int Id)
        {
            int respuesta = await _repositorio.Eliminar(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> ListaAsesoriasAsignadas(int IdEstadoAsesoria)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault()!;

            List<Asesoria> lista = await _repositorio.ListaAsesoriasAsignadas(int.Parse(idUsuario), IdEstadoAsesoria);
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }
        [HttpDelete]
        public async Task<IActionResult> Cancelar(int Id)
        {
            string respuesta = await _repositorioAsesoria.Cancelar(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] Asesoria objeto)
        {
            string respuesta = await _repositorioAsesoria.CambiarEstado(objeto.IdAsesoria, objeto.EstadoAsesoria.IdEstadoAsesoria, objeto.Indicaciones);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }
}
