using ColegioData.Contrato;
using ColegioEntidades;
using ColegioEntidades.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColegioWeb.Controllers
{
    public class AsesoriasController : Controller
    {
        private readonly IDocenteRepositorio _repositorio;
        private readonly IAsesoriaRepositorio _repositorioAsesoria;
        public AsesoriasController(IDocenteRepositorio repositorio, IAsesoriaRepositorio repositorioAsesoria)
        {
            _repositorio = repositorio;
            _repositorioAsesoria = repositorioAsesoria;
        }
        [Authorize(Roles = "Alumno")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Alumno")]
        public IActionResult NuevaAsesoria()
        {
            return View();
        }

        [Authorize(Roles = "Docente")]
        public IActionResult AsesoriasAsignadas()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaDocenteHorarioDetalle(int Id)
        {
            List<FechaAtencionDTO> lista = await _repositorio.ListaDocenteHorarioDetalle(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Asesoria objeto)
        {

            ClaimsPrincipal claimuser = HttpContext.User;
            string idUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault()!;

            objeto.Usuario = new Usuario
            {
                IdUsuario = int.Parse(idUsuario)
            };

            string respuesta = await _repositorioAsesoria.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> ListaAsesoriasPendiente()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault()!;

            List<Asesoria> lista = await _repositorioAsesoria.ListaAsesoriasPendiente(int.Parse(idUsuario));
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpDelete]
        public async Task<IActionResult> Cancelar(int Id)
        {
            string respuesta = await _repositorioAsesoria.Cancelar(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

       
    }
}
