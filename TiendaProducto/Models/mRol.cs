using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaProducto.Models
{
    public class mRol
    {
        [Required]
        [Display(Name ="Rol")]
        public string NombreRol { get; set; }
    }
}
