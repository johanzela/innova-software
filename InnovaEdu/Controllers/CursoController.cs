using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColegioWeb.Controllers
{
    public class CursoController : Controller
    {
        private readonly ICursoRepositorio _repositorio;
        public CursoController(ICursoRepositorio repositorio)
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
            List<Curso> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        public async Task<IActionResult> Guardar([FromBody] Curso objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Curso objeto)
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
    }
}
