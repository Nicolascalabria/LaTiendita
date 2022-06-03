using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaTiendita.Models;
using LaTiendita.Stock;

namespace LaTiendita.Controllers
{
    public class ProductoBisController : Controller
    {
        private readonly BaseDeDatos _context;

        public ProductoBisController(BaseDeDatos context)
        {
            _context = context;
        }

        // GET: ProductoBis
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.ProductoBis.Include(p => p.Categoria);
            return View(await baseDeDatos.ToListAsync());
        }

        // GET: ProductoBis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var productoBis = await _context.ProductoBis
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (productoBis == null)
            {
                return NotFound();
            }

            return View(productoBis);
        }

        // GET: ProductoBis/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // POST: ProductoBis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Precio,Detalle,CategoriaId")] ProductoBis productoBis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoBis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", productoBis.CategoriaId);
            return View(productoBis);
        }

        // GET: ProductoBis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var productoBis = await _context.ProductoBis.FindAsync(id);
            if (productoBis == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", productoBis.CategoriaId);
            return View(productoBis);
        }

        // POST: ProductoBis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Precio,Detalle,CategoriaId")] ProductoBis productoBis)
        {
            if (id != productoBis.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoBis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoBisExists(productoBis.ProductoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", productoBis.CategoriaId);
            return View(productoBis);
        }

        // GET: ProductoBis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var productoBis = await _context.ProductoBis
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (productoBis == null)
            {
                return NotFound();
            }

            return View(productoBis);
        }

        // POST: ProductoBis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductoBis == null)
            {
                return Problem("Entity set 'BaseDeDatos.ProductoBis'  is null.");
            }
            var productoBis = await _context.ProductoBis.FindAsync(id);
            if (productoBis != null)
            {
                _context.ProductoBis.Remove(productoBis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoBisExists(int id)
        {
          return _context.ProductoBis.Any(e => e.ProductoId == id);
        }
    }
}
