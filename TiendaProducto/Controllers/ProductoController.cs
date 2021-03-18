using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaProducto.Data;
using TiendaProducto.Models;

namespace TiendaProducto.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]    
    public class ProductoController : ControllerBase
    {
        private readonly TiendaProductoContext _context;
        private readonly UserManager<mUsuario> _gestionUsuarios;        

        public ProductoController(TiendaProductoContext context, UserManager<mUsuario> gestionUsuarios)
        {
            _context = context;
            _gestionUsuarios = gestionUsuarios;
        }

        // GET: api/Producto
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<mProducto>>> ListarProducto()
        {
            return await _context.mProducto.ToListAsync();
        }

        // GET: api/Producto/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<mProducto>> GetmProducto(int id)
        {
            var mProducto = await _context.mProducto.FindAsync(id);

            if (mProducto == null)
            {
                return NotFound();
            }

            return mProducto;
        }

        // PUT: api/Producto/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles ="Administrador,Vendedor")]
        public async Task<IActionResult> PutmProducto(int id, mProducto mProducto)
        {
            var producto = await _context.mProducto.AnyAsync(x=>x.Usuario==mProducto.Usuario);
            if (id != mProducto.ID)
            {
                return BadRequest();
            }
            var user = await _gestionUsuarios.FindByIdAsync(mProducto.Usuario);
            if (producto || await _gestionUsuarios.IsInRoleAsync(user, "Administrador"))
            {                
                _context.Entry(mProducto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Producto editado con exito");
            }
            else
                return BadRequest("No tiene permiso para editar este producto");         
            
        }

        // POST: api/Producto
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles ="Administrador,Vendedor")]
        public async Task<ActionResult<mProducto>> PostmProducto(mProducto mProducto)
        {
            
            _context.mProducto.Add(mProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmProducto", new { id = mProducto.ID }, mProducto);
        }

        // DELETE: api/Producto/5
        [HttpDelete]
        [Authorize(Roles ="Administrador,Vendedor")]
        public async Task<ActionResult<mProducto>> DeletemProducto(mProducto producto)
        {
            var mProducto = await _context.mProducto.FindAsync(producto.ID);
            var user = await _gestionUsuarios.FindByIdAsync(producto.Usuario);
            if (mProducto==null)
            {
                return NotFound();
            }
            var ismy = await _context.mProducto.AnyAsync(x => x.Usuario == producto.Usuario);
            if (ismy || await _gestionUsuarios.IsInRoleAsync(user,"Administrador"))
            {
                _context.mProducto.Remove(mProducto);
                await _context.SaveChangesAsync();
                return Ok("Producto removido con exito");
            }            

            return BadRequest("No tiene permiso para eliminar este producto");
        }

        private bool mProductoExists(int id)
        {
            return _context.mProducto.Any(e => e.ID == id);
        }

        [HttpPost]
        [Authorize(Roles ="Cliente")]
        public async Task<ActionResult> Comprar(mOrden producto) 
        {
            var productoresult = await _context.mProducto.FindAsync(producto.Producto);
            if (productoresult != null)
            {
                if (producto.Cantidad>0)
                {                
                if (productoresult.Cantidad>producto.Cantidad)
                {
                    productoresult.Cantidad = productoresult.Cantidad - producto.Cantidad;
                   mOrden orden = new mOrden
                    {
                        Estado = Enums.OrdenEstado.Created,
                        Producto = producto.Producto,
                        Fecha = DateTime.Now,
                        Usuario = producto.Usuario,
                        Cantidad = producto.Cantidad
                    };
                    var ordencompra =await _context.mOrden.AddAsync(orden);
                    _context.Entry(productoresult).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Compra realizada con exito, Id de la orden: " + orden.Id);
                }
                    return BadRequest("No existe la cantidad de producto deseada");
                }
                return BadRequest("Elija al menos un producto");
               
                
            }
            return BadRequest("No existe el producto solicitado");
        }

    }
}
