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
    public class CatalogoController : Controller
    {
        private readonly BaseDeDatos _context;

        public CatalogoController(BaseDeDatos context)
        {
            _context = context;
        }

        // GET: Catalogo
        public async Task<IActionResult> Index()
        {
            var baseDeDatos = _context.ProductoBis.Include(p => p.Categoria);
            return View(await baseDeDatos.ToListAsync());
        }

        // GET: Catalogo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductoBis
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Catalogo/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Catalogo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Precio,Detalle,CategoriaId")] ProductoBis producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // GET: Catalogo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductoBis.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // POST: Catalogo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Precio,Detalle,CategoriaId")] ProductoBis producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", producto.CategoriaId);
            return View(producto);
        }

        // GET: Catalogo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductoBis == null)
            {
                return NotFound();
            }

            var producto = await _context.ProductoBis
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Catalogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductoBis == null)
            {
                return Problem("Entity set 'BaseDeDatos.Productos'  is null.");
            }
            var producto = await _context.ProductoBis.FindAsync(id);
            if (producto != null)
            {
                _context.ProductoBis.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return _context.ProductoBis.Any(e => e.ProductoId == id);
        }
    }
}
