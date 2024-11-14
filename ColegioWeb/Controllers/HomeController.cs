using ColegioWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using ColegioData.Contrato;
using ColegioEntidades.DTO;
using System.Security.Claims;
using ColegioEntidades;


namespace ColegioWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardRepositorio _repositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HomeController(ILogger<HomeController> logger, IDashboardRepositorio respositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            _repositorio = respositorio;
            _usuarioRepositorio= usuarioRepositorio;

        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Dashboard()
        {
            DashboardDTO objeto = await _repositorio.Obtener();
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        public async Task<IActionResult> Salir()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                await _usuarioRepositorio.Desconectar(userId);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}
