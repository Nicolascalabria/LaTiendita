using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authorization;

namespace LaTiendita.Controllers
{
    [Authorize(Roles = "ADMIN")]
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
            ViewData["TalleId"] = new SelectList(_context.ProductoTalle, "TalleId", "Nombre", productoBis.ProductoTalle);
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

        private bool talleExist(String nombre)
        {
            return _context.ProductoBis.Any(e => e.Nombre == nombre);
        }


        private void ValidarTalle (String nombre)
        {

            bool talleValido = nombre.ToUpper().Equals("S") ||
                               nombre.ToUpper().Equals("M") ||
                               nombre.ToUpper().Equals("L") ||
                               nombre.ToUpper().Equals("XL");


            if (!talleValido)
            {
                throw new Exception("El talle ingresado no es valido");
            }

        }



        [HttpPost, ActionName("AddTalle")]
        public IActionResult AddTalle(int id, String nombre, int cantidad)
        {
            if (id > 0)
            {
                try
                {
                    ValidarTalle(nombre);

                    var talle = _context
                        .Talles
                        .Where(o => o.Nombre.ToUpper().Equals(nombre.ToUpper())).FirstOrDefault();

                    var talleProducto = _context
                        .ProductoTalle
                        .Where(o => o.TalleId == talle.TalleId && o.ProductoId == id).FirstOrDefault();

                    if (talleProducto == null)
                    {

                        _context.ProductoTalle.Add(new ProductoTalle()
                        {
                            ProductoId = id,
                            Cantidad = cantidad,
                            TalleId = talle.TalleId,

                        });
                        _context.SaveChanges();

                    }
                    else
                    {
                        talleProducto.Cantidad += cantidad;
                        _context.SaveChanges();
                    }

                    return Ok();


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            else
            {
                throw new Exception("El Id de cita esta mal...");
            }
            //return Json(new { success = false, responseText = "The attached file is not supported." });
            //return Ok();
        }
          
    }
}
    