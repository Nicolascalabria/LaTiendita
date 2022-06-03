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
    public class ProductoTallesController : Controller
    {
        private readonly BaseDeDatos _context;

        public ProductoTallesController(BaseDeDatos context)
        {
            _context = context;
        }

        // GET: ProductoTalles
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.ProductoTalle.Include(p => p.Talle).Include(p => p.producto);
            return View(await baseDeDatos.ToListAsync());
        }

        // GET: ProductoTalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductoTalle == null)
            {
                return NotFound();
            }

            var productoTalle = await _context.ProductoTalle
                .Include(p => p.Talle)
                .Include(p => p.producto)
                .FirstOrDefaultAsync(m => m.ProductoTalleId == id);
            if (productoTalle == null)
            {
                return NotFound();
            }

            return View(productoTalle);
        }

        // GET: ProductoTalles/Create
        public IActionResult Create()
        {
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "TalleId");
            ViewData["ProductoId"] = new SelectList(_context.Set<ProductoBis>(), "ProductoId", "ProductoId");
            return View();
        }

        // POST: ProductoTalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoTalleId,TalleId,ProductoId,Cantidad")] ProductoTalle productoTalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoTalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "TalleId", productoTalle.TalleId);
            ViewData["ProductoId"] = new SelectList(_context.Set<ProductoBis>(), "ProductoId", "ProductoId", productoTalle.ProductoId);
            return View(productoTalle);
        }

        // GET: ProductoTalles/Edit/5
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
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "TalleId", productoTalle.TalleId);
            ViewData["ProductoId"] = new SelectList(_context.Set<ProductoBis>(), "ProductoId", "ProductoId", productoTalle.ProductoId);
            return View(productoTalle);
        }

        // POST: ProductoTalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoTalleId,TalleId,ProductoId,Cantidad")] ProductoTalle productoTalle)
        {
            if (id != productoTalle.ProductoTalleId)
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
                    if (!ProductoTalleExists(productoTalle.ProductoTalleId))
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
            ViewData["TalleId"] = new SelectList(_context.Talles, "TalleId", "TalleId", productoTalle.TalleId);
            ViewData["ProductoId"] = new SelectList(_context.Set<ProductoBis>(), "ProductoId", "ProductoId", productoTalle.ProductoId);
            return View(productoTalle);
        }

        // GET: ProductoTalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductoTalle == null)
            {
                return NotFound();
            }

            var productoTalle = await _context.ProductoTalle
                .Include(p => p.Talle)
                .Include(p => p.producto)
                .FirstOrDefaultAsync(m => m.ProductoTalleId == id);
            if (productoTalle == null)
            {
                return NotFound();
            }

            return View(productoTalle);
        }

        // POST: ProductoTalles/Delete/5
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
          return _context.ProductoTalle.Any(e => e.ProductoTalleId == id);
        }
    }
}
