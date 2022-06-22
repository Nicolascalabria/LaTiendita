using LaTiendita.Models;
using LaTiendita.Models.Enums;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace LaTiendita.Controllers
{
    public class HomeController : Controller
    {
        private readonly BaseDeDatos _context;

        public HomeController(BaseDeDatos context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index2");

        }

        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            var usuario = await _context.Usuarios
               .FirstOrDefaultAsync(o => o.Email.ToUpper().Equals(email.ToUpper()));

            if (usuario is null)
                return RedirectToAction("CrearUsuarioNoAdmin", "Usuarios");

            Loguearse(usuario);

            return usuario.Rol switch
            {
                Roles.Administrador => RedirectToAction("Index", "Producto"),
                _ => RedirectToAction("Index", "Catalogo"),
            };
        }

        public async Task<IActionResult> LimpiarCarrito()
        {
            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if (carritoDb != null)
            {
                _context.Carritos.Remove(carritoDb);
                var productos = _context.CarritoProducto
                        .ToList();
                _context.CarritoProducto
                .RemoveRange(productos);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await LimpiarCarrito();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private void Loguearse(Usuario usuario)
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
            identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}