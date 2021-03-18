using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaProducto.Models;

namespace TiendaProducto.Controllers
{
    [Authorize(Roles ="Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministracionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> gestionRoles;

        public AdministracionController(RoleManager<IdentityRole> gestionRoles)
        {
            this.gestionRoles = gestionRoles;
        }
        [HttpPost]
        public async Task<ActionResult> CrearRol(mRol rol)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = rol.NombreRol
                };

                IdentityResult result = await gestionRoles.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok("Rol creado correctamente");
                }

                return BadRequest(result.Errors);
            }
            return BadRequest("Datos no valido");
        }
        [HttpGet]
        public ActionResult GetRoles() 
        {
            var roles = gestionRoles.Roles;
            return Ok(roles);
        }
    }
}
