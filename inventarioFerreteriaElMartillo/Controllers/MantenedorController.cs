using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using inventarioFerreteriaElMartillo.Models;

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Newtonsoft.Json;

namespace inventarioFerreteriaElMartillo.Controllers
{
    public class MantenedorController : Controller
    {

        IFirebaseClient cliente;

        public MantenedorController()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                BasePath = "https://inventario-martillo-default-rtdb.firebaseio.com/"
            };

            cliente = new FirebaseClient(config);
   
        }

        // GET: Mantenedor
        public ActionResult Inicio()
        {
            Dictionary<string, Producto> lista = new Dictionary<string, Producto>();
            FirebaseResponse response = cliente.Get("inventario/");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Producto>>(response.Body);

            List<Producto> productos = new List<Producto>();

            foreach (KeyValuePair<string,Producto> item in lista)
            {
                productos.Add(new Producto()
                {
                    IdProducto = item.Key,
                    Articulo = item.Value.Articulo,
                    Descripcion = item.Value.Descripcion,
                    Marca = item.Value.Marca,
                    Precio = item.Value.Precio,
                    Stock = item.Value.Stock,
                    Imagen = item.Value.Imagen,
                });
            }

            return View(productos);  
        }
        public ActionResult Crear()
        {
            return View();
        }
        public ActionResult Editar(string idProducto)
        {
            FirebaseResponse response = cliente.Get("inventario/" + idProducto);

            Producto producto = response.ResultAs<Producto>();
            producto.IdProducto = idProducto;

            return View(producto);
        }
        public ActionResult Eliminar(string idProducto)
        {

            FirebaseResponse response = cliente.Delete("inventario/" + idProducto);

            return RedirectToAction("Inicio", "Mantenedor");
        }


        [HttpPost]
        public ActionResult Crear(Producto producto)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");

            SetResponse response = cliente.Set("inventario/" + IdGenerado, producto);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("Error", "Error de insercion en la base de datos");
                return View(producto);
            }
        }

        [HttpPost]
        public ActionResult Editar(Producto producto)
        {

            string idProducto = producto.IdProducto;
            producto.IdProducto = null;

            FirebaseResponse response = cliente.Update("inventario/" + idProducto, producto);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Inicio", "Mantenedor");
            }
            else
            {
                ModelState.AddModelError("Error", "Error de insercion en la base de datos");
                return View(producto);
            }
        }
    }
}