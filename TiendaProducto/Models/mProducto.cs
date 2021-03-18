using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaProducto.Models
{
    public class mProducto
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(100,MinimumLength =10)]
        public string Descripcion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Cantidad { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Precio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Usuario { get; set; }
    }
}
