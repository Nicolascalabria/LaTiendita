using LaTiendita.Models;
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
            return View();

        }

        [HttpPost]
        public IActionResult Index(string email)
        {
            var usuario = _context
               .Usuarios
               .Where(o => o.Email.ToUpper().Equals(email.ToUpper()))
                .FirstOrDefault();

            if (email == "gabrielarce@gmail.com")
            {
                LoguearseAdmin(usuario);
                return RedirectToAction("Index", "Producto");
            }
            else
            {

                //Quiero saber si en la base existe, sino tengo que crearlo
                bool usuarioExiste = usuario != null;

                if (usuarioExiste)
                {
                    LoguearseUsuario(usuario);
                    return RedirectToAction("Index", "Catalogo");
                }
                else
                {
                    return RedirectToAction("Registrarse", "Usuarios", email);
                }

            }
        }

        private void LoguearseUsuario(Usuario usuario)
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
            identity.AddClaim(new Claim(ClaimTypes.Role, "USUARIO"));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private void LoguearseAdmin(Usuario usuario)
        {
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
            identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
            identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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
            
            await VaciarCarrito();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
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


        [HttpPost, ActionName("DevolverStock")]
        private async Task<IActionResult> DevolverStock(CarritoProducto productoARecuperar)
        {


            var productoTalle = await _context.ProductoTalle
                .FirstOrDefaultAsync(x => x.ProductoId == productoARecuperar.ProductoId && x.TalleId == productoARecuperar.TalleId);


            productoTalle.Cantidad += productoARecuperar.Cantidad;
            _context.ProductoTalle.Update(productoTalle);


            await _context.SaveChangesAsync();

            return Ok();
        }


        public async Task<IActionResult> VaciarCarrito()
        {


            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if (carritoDb != null)
            {

                var productos = _context.CarritoProducto
                        .ToList();

                foreach (var item in productos)
                {
                    _ = DevolverStock(item);
                }

                _ = LimpiarCarrito();

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Carrito");

        }




    }


}