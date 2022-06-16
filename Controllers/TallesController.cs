#nullable disable
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
    public class TallesController : Controller
    {
        private readonly BaseDeDatos _context;

        public TallesController(BaseDeDatos context)
        {
            _context = context;
        }

        // GET: Talles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Talles.ToListAsync());
        }

        // GET: Talles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talle = await _context.Talles
                .FirstOrDefaultAsync(m => m.TalleId == id);
            if (talle == null)
            {
                return NotFound();
            }

            return View(talle);
        }

        // GET: Talles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Talles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Talles/Edit/5
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

        // POST: Talles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TalleId,Nombre")] Talle talle)
        {
            if (id != talle.TalleId)
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
                    if (!TalleExists(talle.TalleId))
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

        // GET: Talles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talle = await _context.Talles
                .FirstOrDefaultAsync(m => m.TalleId == id);
            if (talle == null)
            {
                return NotFound();
            }

            return View(talle);
        }

        // POST: Talles/Delete/5
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
            return _context.Talles.Any(e => e.TalleId == id);
        }
    }
}
