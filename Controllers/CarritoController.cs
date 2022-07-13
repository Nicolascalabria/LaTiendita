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
        public async Task<ActionResult<bool>> AgregarProductoAlCarrito(int productoId, int talleId, int cantidad)
        {
            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .Include(x => x.Productos)
                    .ThenInclude(x => x.Talle)
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if (carritoDb is null)
            {
                carritoDb = new Carrito() { UsuarioId = cookieUserId };
                _context.Carritos.Add(carritoDb);
                await _context.SaveChangesAsync();
            }

            //if (!await ProductoExists(productoId))
            //    return NotFound();      

            //if (!await TalleExists(talleId))
            //    return NotFound();
            //    
            if(!HayStock(productoId, talleId))
            {
                TempData["msgError"] = "No hay stock del producto seleccionado.";
                return false;
            }

            var productosDelCarrito = carritoDb.Productos
                .Any(x => x.ProductoId == productoId && x.TalleId == talleId);

            var hayStock =  HayStock(productoId, talleId);

            var productoEnStock =  _context.ProductoTalle.FirstOrDefault(e => e.ProductoId == productoId && e.TalleId == talleId);

            if (!productosDelCarrito)
            {              
                    carritoDb.Productos.Add(new CarritoProducto() { ProductoId = productoId, TalleId = talleId, Cantidad = cantidad });
                    productoEnStock.Cantidad -= cantidad;
                    _context.ProductoTalle.Update(productoEnStock);                   
                    TempData["msgOk"] = "Se agrego al chango";
                                         
            }
            else
            {
                    var productoTalle = carritoDb.Productos.FirstOrDefault(x => x.ProductoId == productoId && x.TalleId == talleId);
                    productoTalle.Cantidad += cantidad;
                    productoEnStock.Cantidad -= cantidad;
                    _context.Carritos.Update(carritoDb);
                    _context.ProductoTalle.Update(productoEnStock);    
                    TempData["msgOk"] = "Se agrego al chango";
                              
                            

            }
                    await _context.SaveChangesAsync();

            return Ok();
        }


        private bool HayStock(int productoId, int talleId)
        {
            var hayStock = false;
            ProductoTalle productoAchequear = _context.ProductoTalle.FirstOrDefault(e => e.ProductoId == productoId && e.TalleId == talleId);

            if(productoAchequear != null && productoAchequear.Cantidad > 0)
            {
                hayStock = true;
            }
                
            return  hayStock;
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


        [HttpPost, ActionName("DevolverStock")]
        private async Task<IActionResult> DevolverStock(CarritoProducto productoARecuperar)
        {
            

            var productoTalle = await _context.ProductoTalle
                .FirstOrDefaultAsync(x => x.ProductoId == productoARecuperar.ProductoId && x.TalleId == productoARecuperar.TalleId);

           
                productoTalle.Cantidad += productoARecuperar.Cantidad;
                _context.ProductoTalle.Update(productoTalle);
           

            await _context.SaveChangesAsync();

            return Ok();
        }
        

        public async Task<IActionResult> VaciarCarrito()
        {


            var cookieUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoDb = await _context.Carritos
                .FirstOrDefaultAsync(x => x.UsuarioId == cookieUserId);

            if (carritoDb != null)
            {

                var productos = _context.CarritoProducto
                        .ToList();

                foreach (var item in productos)
                {
                    _ = DevolverStock(item);
                }

                _ = LimpiarCarrito();

                await _context.SaveChangesAsync();
            }

                return RedirectToAction("Index", "Carrito");

            

        }

        public async Task<IActionResult> Comprar()
        {
            var productosEnChango = await _context.CarritoProducto.ToListAsync();

            if(productosEnChango.Count > 0)
            {
                await LimpiarCarrito();

                return View();
            }


            return RedirectToAction("Index");



        }

    }
}
