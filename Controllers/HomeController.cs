using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(string email)
        {
            var usuario = _context
               .Usuarios
               .Where(o => o.Email.ToUpper().Equals(email.ToUpper()))
                .FirstOrDefault();

            if (email == "eduardomass@gmail.com" || email == "nicolasgcalabria@gmail.com")
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                //Lo q obtengo al acceder a User.Identity.Name
                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
                //Se Usara para autorizacion por roles
                identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
                //Se usa para acceder al ID del usuario en sistema
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Email.ToString()));
                //Lo usamos cuando queremos mostrar el nombre del Usuario logueado en sistema
                identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                //En este apso se hace el login del usuario al sistema
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                return RedirectToAction("Index", "Productoes");
            }
            else
            {

                //Quiero saber si en la base existe, sino tengo que crearlo
                bool usuarioExiste = usuario != null;

                if (usuarioExiste)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    //Lo q obtengo al acceder a User.Identity.Name
                    identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
                    //Se Usara para autorizacion por roles
                    identity.AddClaim(new Claim(ClaimTypes.Role, "USUARIO"));
                    //Se usa para acceder al ID del usuario en sistema
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()));
                    //Lo usamos cuando queremos mostrar el nombre del Usuario logueado en sistema
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    //En este apso se hace el login del usuario al sistema
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                    return RedirectToAction("Index", "Catalogo");
                }
                else
                {
                    return RedirectToAction("Create", "Usuarios");
                }

            }
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