using LaTiendita.Models;
using LaTiendita.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LaTiendita.Controllers
{
    public class CarritoController : Controller
    {
        private readonly BaseDeDatos _context;

        public CarritoController(BaseDeDatos context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            

            var productos = await _context.CarritoProducto
               .Include(x => x.Producto)
               .Include(p => p.Talle)
               .ToListAsync();
            ViewData["TotalCarrito"] = calcularTotal();
            return View(productos);
        }




        [HttpPost, ActionName("AddProduct")]
        public async Task<IActionResult> AgregarProductoAlCarrito(int productoId, int talleId, int cantidad)
        {
            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .Include(x => x.Productos)
                    .ThenInclude(x => x.Talle)
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if(carritoDb is null)
            {
                carritoDb = new Carrito() { UsuarioId = cookieUserId };
                _context.Carritos.Add(carritoDb);
                await _context.SaveChangesAsync();
            }

            if (!await ProductoExists(productoId))
                return NotFound();      

            if (!await TalleExists(talleId))
                return NotFound();      

            var productosDelCarrito = carritoDb.Productos
                .Any(x => x.ProductoId == productoId && x.TalleId == talleId);

            if (!productosDelCarrito)
                carritoDb.Productos.Add(new CarritoProducto() { ProductoId = productoId, TalleId = talleId, Cantidad = cantidad });
            else
            {
                var productoTalle = carritoDb.Productos.FirstOrDefault(x => x.ProductoId == productoId && x.TalleId == talleId);
                productoTalle.Cantidad += cantidad;
                _context.Carritos.Update(carritoDb);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _context.Producto.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> TalleExists(int id)
        {
            return await _context.Talles.AnyAsync(e => e.Id == id);
        }

        private double calcularTotal()
        {


            var carritoList = _context.CarritoProducto.ToList();

            double total=0;
            foreach (var carrito in carritoList)
            {
                total += carrito.Cantidad * carrito.Producto.Precio;
            }   
            

            return total;
        }

        public async Task<IActionResult> LimpiarCarrito()
        {
            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if (carritoDb != null)
            {
                _context.Carritos.Remove(carritoDb);
                var productos = _context.CarritoProducto
                        .ToList();
                _context.CarritoProducto
                .RemoveRange(productos);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index", "Carrito");

            }
            
            return RedirectToAction("Index", "Carrito");    
        }

    }
}
