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
            
           
            return View();
        }

        // GET: ProductosController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
           
            return View();
        }

        // POST: ProductosController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, String nombre, int precio, String detalle)
        {
            try
            {
                
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
            
            return View();
        }

        // POST: ProductosController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
