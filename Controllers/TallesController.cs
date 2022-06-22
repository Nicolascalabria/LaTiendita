#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authorization;

namespace LaTiendita.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TallesController : Controller
    {
        private readonly BaseDeDatos _context;

        public TallesController(BaseDeDatos context)
        {
            _context = context;
        }

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
            if (ModelState.IsValid)
            {
                _context.Add(talle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(talle);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var talle = await _context.Talles.FindAsync(id);
            _context.Talles.Remove(talle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TalleExists(int id)
        {
            return _context.Talles.Any(e => e.Id == id);
        }
    }
}
