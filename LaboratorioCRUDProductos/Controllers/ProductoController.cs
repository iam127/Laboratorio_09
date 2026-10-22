using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LaboratorioCRUDProductos.Models;

namespace LaboratorioCRUDProductos.Controllers
{
    public class ProductoController : Controller
    {
        // Obtiene la lista desde la sesion. Solo la inicializa si todavia no existe.
        private List<ProductoModel> ObtenerProductos()
        {
            var productos = Session["productos"] as List<ProductoModel>;

            if (productos == null)
            {
                productos = new List<ProductoModel>
                {
                    new ProductoModel { Id = 1, Nombre = "Laptop Lenovo", Descripcion = "Laptop 15 pulgadas, 16 GB RAM", Precio = 2899.90m, Stock = 10 },
                    new ProductoModel { Id = 2, Nombre = "Mouse Logitech", Descripcion = "Mouse inalámbrico ergonómico", Precio = 79.50m, Stock = 45 },
                    new ProductoModel { Id = 3, Nombre = "Teclado Mecánico", Descripcion = "Teclado retroiluminado switch rojo", Precio = 189.00m, Stock = 25 }
                };
                Session["productos"] = productos;
            }

            return productos;
        }

        // GET Producto
        public ActionResult Index()
        {
            return View(ObtenerProductos());
        }

        // GET Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        // GET Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoModel producto)
        {
            if (!ModelState.IsValid) return View(producto);

            var productos = ObtenerProductos();
            producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
            productos.Add(producto);
            Session["productos"] = productos;

            return RedirectToAction("Index");
        }

        // GET Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        // POST Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductoModel producto)
        {
            if (!ModelState.IsValid) return View(producto);

            var productos = ObtenerProductos();
            var existente = productos.FirstOrDefault(p => p.Id == producto.Id);
            if (existente == null) return HttpNotFound();

            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;
            Session["productos"] = productos;

            return RedirectToAction("Index");
        }

        // GET Producto/Delete/5 (pantalla de confirmacion)
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        // POST Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var productos = ObtenerProductos();
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto != null)
            {
                productos.Remove(producto);
                Session["productos"] = productos;
            }

            return RedirectToAction("Index");
        }
    }
}