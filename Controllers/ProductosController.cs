using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LaTiendita.Models;
using LaTiendita.Stock;


namespace LaTiendita.Controllers
{
    public class ProductosController : Controller
    {
        // GET: ProductosController1
        public ActionResult Index()
        {
            
           
            return View(BaseDeDatos.listaProductos);
        }

        // GET: ProductosController1/Details/5
        public ActionResult Details(int id)
        {
            Producto productoDetalle = BaseDeDatos.listaProductos.Where(prod => prod.CodigoProducto == id).FirstOrDefault();
            return View(productoDetalle);
        }

        // GET: ProductosController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductosController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto)
        {
            try
            {

                producto.CodigoProducto = BaseDeDatos.listaProductos.Count + 1;
                BaseDeDatos.listaProductos.Add(producto);  
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController1/Edit/5
        public ActionResult Edit(int id)
        {
            Producto productoAEditar = BaseDeDatos.listaProductos.Where(prod => prod.CodigoProducto == id).FirstOrDefault();
            return View(productoAEditar);
        }

        // POST: ProductosController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, String nombre, int precio, String detalle)
        {
            try
            {
                Producto productoAEditar = BaseDeDatos.listaProductos.Where(prod => prod.CodigoProducto == id).FirstOrDefault();
                productoAEditar.Nombre = nombre;
                productoAEditar.Precio = precio;
                productoAEditar.Detalle = detalle;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController1/Delete/5
        public ActionResult Delete(int id)
        {
            Producto productoABorrar = BaseDeDatos.listaProductos.Where(prod => prod.CodigoProducto == id).FirstOrDefault();
            return View(productoABorrar);
        }

        // POST: ProductosController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Producto productoABorrar = BaseDeDatos.listaProductos.Where(prod => prod.CodigoProducto == id).FirstOrDefault(); 
                BaseDeDatos.listaProductos.Remove(productoABorrar); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
