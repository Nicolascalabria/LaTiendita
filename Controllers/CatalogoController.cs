using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using LaTiendita.Stock;
using LaTiendita.Models;

namespace LaTiendita.Controllers
{
    
    public class CatalogoController : Controller
    {
        private readonly BaseDeDatos _context;

        public CatalogoController(BaseDeDatos context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Producto
                .Include(x => x.Talles)
                .Include(p => p.Categoria)
                .ToListAsync();

            ViewData["Talles"] = new SelectList(_context.Talles, "Id", "Nombre");
            return View(productos);
        }

      
    }
}
