using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColegioWeb.Controllers
{
    public class HistorialAsesoriasController : Controller
    {
        private readonly IAsesoriaRepositorio _repositorioAsesoria;
        public HistorialAsesoriasController(IAsesoriaRepositorio repositorioAsesoria)
        {
            _repositorioAsesoria = repositorioAsesoria;
        }
        [Authorize(Roles = "Alumno")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaHistorialAsesorias()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault()!;

            List<Asesoria> lista = await _repositorioAsesoria.ListaHistorialAsesorias(int.Parse(idUsuario));
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }
    }
}
