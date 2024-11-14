using ColegioEntidades;
using ColegioWeb.Models.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ColegioData.Contrato;
using ColegioWeb.Services;
using System.Net.Mail;
using System.Net;

namespace ColegioWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioRepositorio _repositorio;
        public AccesoController(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            if (claimuser.Identity!.IsAuthenticated)
            {
                string rol = claimuser.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault()!;
                if (rol == "Administrador") return RedirectToAction("Index", "Home");
                if (rol == "Alumno") return RedirectToAction("Index", "Asesorias");
                if (rol == "Docente") return RedirectToAction("Index", "Home");
                if (rol == "Recepcionista") return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            if (modelo.DocumentoIdentidad == null || modelo.Clave == null)
            {
                ViewData["Mensaje"] = "por favor llene todos los campos";
                return View();
            }

            Usuario usuario_encontrado = await _repositorio.Login(modelo.DocumentoIdentidad, modelo.Clave);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "por favor revisar usuario y contraseña";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{usuario_encontrado.Nombre} {usuario_encontrado.Apellido}"),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role,usuario_encontrado.RolUsuario.Nombre)
            };


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            string rol = usuario_encontrado.RolUsuario.Nombre;
            if (rol == "Alumno") return RedirectToAction("Index", "Asesorias");
            if (rol == "Docente") return RedirectToAction("AsesoriasAsignadas", "Asesorias");
            if (rol == "Recepcionista") return RedirectToAction("Index", "Asesorias");

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(VMAlumno modelo)
        {
            if (modelo.Clave != modelo.ConfirmarClave)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden";
                return View();
            }

            Usuario objeto = new Usuario()
            {
                NumeroDocumentoIdentidad = modelo.DocumentoIdentidad,
                Nombre = modelo.Nombre,
                Apellido = modelo.Apellido,
                Correo = modelo.Correo,
                Clave = modelo.Clave,
                RolUsuario = new RolUsuario()
                {
                    IdRolUsuario = 2
                }
            };
            string resultado = await _repositorio.Guardar(objeto);
            ViewBag.Mensaje = resultado;
            if (resultado == "")
            {
                ViewBag.Creado = true;
                ViewBag.Mensaje = "Su cuenta ha sido creada.";
            }


            return View();
        }

        public IActionResult Denegado()
        {
            return View();
        }
    }
}
