using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventarioFerreteriaElMartillo.Models
{
    public class Producto
    {
        public string IdProducto { get; set; }
        public string Articulo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set;}
        public string Marca { get; set; }
        public string Precio { get; set; }
        public string Stock { get; set; }
    
    }
}