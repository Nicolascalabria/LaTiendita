using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using LaTiendita.Models;
using LaTiendita.Stock;

namespace LaTiendita.Controllers
{
    
    public class ProductoController : Controller
    {
        private readonly BaseDeDatos _context;

        public ProductoController(BaseDeDatos context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.Producto.Include(p => p.Categoria);
            return View(await baseDeDatos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var Producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Producto == null)
            {
                return NotFound();
            }

            return View(Producto);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Precio,Detalle,CategoriaId")] Producto Producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", Producto.CategoriaId);
            return View(Producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var Producto = await _context.Producto
                .Include(x => x.Talles)
                    .ThenInclude(x => x.Talle)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            if (Producto == null)
            {
                return NotFound();
            }
            
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", Producto.CategoriaId);
            ViewData["Talles"] = new SelectList(_context.Talles, "Id", "Nombre", Producto.CategoriaId);
            return View(Producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, ProductoId, Nombre, Precio, Detalle, CategoriaId")] Producto Producto)
        {
            if (id != Producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProductoExists(Producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", Producto.CategoriaId);
            ViewData["TalleId"] = new SelectList(_context.ProductoTalle, "TalleId", "Nombre", Producto.Talles);
            return View(Producto);
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var Producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Producto == null)
            {
                return NotFound();
            }

            return View(Producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producto == null)
            {
                return Problem("Entity set 'BaseDeDatos.Producto'  is null.");
            }
            var Producto = await _context.Producto.FindAsync(id);
            if (Producto != null)
            {
                _context.Producto.Remove(Producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _context.Producto.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> TalleExists (int id)
        {
            return await _context.Talles.AnyAsync(e => e.Id == id);
        }



        [HttpPost, ActionName("AddTalle")]
        public async Task<IActionResult> AddTalle(int productoId, int talleId, int cantidad)
        {
            if (! await ProductoExists(productoId))
                return NotFound();

            if (! await TalleExists(talleId))
                return NotFound();

            var productoTalle = await _context.ProductoTalle
                .FirstOrDefaultAsync(x => x.ProductoId == productoId && x.TalleId == talleId);

            if(productoTalle is null)
                _context.ProductoTalle.Add(new ProductoTalle() { ProductoId = productoId, TalleId = talleId, Cantidad = cantidad });
            else
            {
                productoTalle.Cantidad += cantidad;
                _context.ProductoTalle.Update(productoTalle);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
    