using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaProducto.Models
{
    public class mRegistro
    {
        [Required(ErrorMessage ="Campo Requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage ="Campo requerido")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Campo requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="El password y el pasword de confirmacion no coinciden.")]
        public string PasswordValidar { get; set; }
        [Required(ErrorMessage ="Campo requerido")]
        public string Username { get; set; }


    }
}
