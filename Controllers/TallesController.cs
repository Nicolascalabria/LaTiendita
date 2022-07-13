#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authorization;

namespace LaTiendita.Controllers
{
    
    public class TallesController : Controller
    {
        private readonly BaseDeDatos _context;

        public TallesController(BaseDeDatos context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Talles.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talle = await _context.Talles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talle == null)
            {
                return NotFound();
            }

            return View(talle);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TalleId,Nombre")] Talle talle)
        {
            if (!await ExisteTalle(talle.Nombre))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(talle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
               
            }
            TempData["msgError"] = "El talle ya existe";
            return RedirectToAction("Create", "Talles");

        }

       

        private async Task<bool> ExisteTalle(string nombre)
        {

            var nombreTalle = await _context.Talles
               .AnyAsync(m => m.Nombre.ToUpper() == nombre.ToUpper());

            return nombreTalle; 
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talle = await _context.Talles.FindAsync(id);
            if (talle == null)
            {
                return NotFound();
            }
            return View(talle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, TalleId, Nombre")] Talle talle)
        {
            if (id != talle.Id)
            {
                return NotFound();
            }

            if (!await ExisteTalle(talle.Nombre) && !await TieneStock(id))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(talle);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TalleExists(talle.Id))
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
                return View(talle);
            }
            TempData["msgEdicion"] = "No se puede editar un talle con stock";
            return RedirectToAction("Edit", "Talles");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var talle = await _context.Talles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talle == null)
            {
                return NotFound();
            }

            return View(talle);
        }

     

        private async Task<bool> TieneStock(int id)
        {
            var tieneStock= await _context.ProductoTalle
               .AnyAsync(x => x.TalleId == id && x.Cantidad > 0);

            return tieneStock;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var productosAChequear = await _context.ProductoTalle
            //   .Where(x => x.TalleId == id)
            //   .ToListAsync();

            //var productosAChequearDos = await _context.ProductoTalle
            //   .AnyAsync(x => x.TalleId == id  && x.Cantidad > 0);


            if (!await TieneStock(id))
            {
                var talle = await _context.Talles.FindAsync(id);
                _context.Talles.Remove(talle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["msgBorrado"] = "No se puede eliminar un talle con  productos en stock";
                return RedirectToAction(nameof(Index));

            }
        }

        public async Task<IActionResult> TalleConStock()
        {
            return View(await _context.Talles.ToListAsync());
        }

        private bool TalleExists(int id)
        {
            return _context.Talles.Any(e => e.Id == id);
        }
    }
}
