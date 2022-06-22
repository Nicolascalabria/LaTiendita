using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using LaTiendita.Models;
using LaTiendita.Stock;

namespace LaTiendita.Controllers
{
    
    public class ProductoTallesController : Controller
    {
        private readonly BaseDeDatos _context;

        public ProductoTallesController(BaseDeDatos context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.ProductoTalle.Include(p => p.Producto).Include(p => p.Talle);
            return View(await baseDeDatos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductoTalle == null)
            {
                return NotFound();
            }

            var productoTalle = await _context.ProductoTalle
                .Include(p => p.Producto)
                .Include(p => p.Talle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoTalle == null)
            {
                return NotFound();
            }

            return View(productoTalle);
        }


        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Producto, "ProductoId", "Nombre");
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TalleId,ProductoId,Cantidad")] ProductoTalle productoTalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoTalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "ProductoId", "Nombre", productoTalle.ProductoId);
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "Nombre", productoTalle.TalleId);
            return View(productoTalle);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductoTalle == null)
            {
                return NotFound();
            }

            var productoTalle = await _context.ProductoTalle.FindAsync(id);
            if (productoTalle == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "ProductoId", "Nombre", productoTalle.ProductoId);
            
            return View(productoTalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TalleId,ProductoId,Cantidad")] ProductoTalle productoTalle)
        {
            if (id != productoTalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoTalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoTalleExists(productoTalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Producto, "ProductoId", "Nombre", productoTalle.ProductoId);
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "Nombre", productoTalle.TalleId);
            return View(productoTalle);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductoTalle == null)
            {
                return NotFound();
            }

            var productoTalle = await _context.ProductoTalle
                .Include(p => p.Producto)
                .Include(p => p.Talle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productoTalle == null)
            {
                return NotFound();
            }

            return View(productoTalle);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductoTalle == null)
            {
                return Problem("Entity set 'BaseDeDatos.ProductoTalle'  is null.");
            }
            var productoTalle = await _context.ProductoTalle.FindAsync(id);
            if (productoTalle != null)
            {
                _context.ProductoTalle.Remove(productoTalle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoTalleExists(int id)
        {
          return _context.ProductoTalle.Any(e => e.Id == id);
        }
    }
}
