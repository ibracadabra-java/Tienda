using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaProducto.Enums;

namespace TiendaProducto.Models
{
    public class mOrden
    {
        public int Id { get; set; }
        public int Producto { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public OrdenEstado Estado { get; set; }
        public int Cantidad { get; set; }
    }
}
