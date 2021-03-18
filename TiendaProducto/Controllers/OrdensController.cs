using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaProducto.Enums;
using TiendaProducto.Data;
using TiendaProducto.Models;
using Microsoft.AspNetCore.Authorization;

namespace TiendaProducto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdensController : ControllerBase
    {
        private readonly TiendaProductoContext _context;

        public OrdensController(TiendaProductoContext context)
        {
            _context = context;
        }

        // GET: api/Ordens
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<mOrden>>> GetmOrden()
        {
            return await _context.mOrden.ToListAsync();
        }

        // GET: api/Ordens/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<mOrden>> GetmOrden(int id)
        {
            var mOrden = await _context.mOrden.FindAsync(id);

            if (mOrden == null)
            {
                return NotFound();
            }

            return mOrden;
        }

        // PUT: api/Ordens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutmOrden(int id,OrdenEstado estado)
        {
            var orden = await _context.mOrden.FindAsync(id);
            orden.Estado = estado;
            _context.Entry(orden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Orden actualizada con exito.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mOrdenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent(); 
                }
            }

            
        }

        // POST: api/Ordens
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<mOrden>> PostmOrden(mOrden mOrden)
        {
            _context.mOrden.Add(mOrden);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmOrden", new { id = mOrden.Id }, mOrden);
        }

        // DELETE: api/Ordens/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<mOrden>> DeletemOrden(int id)
        {
            var mOrden = await _context.mOrden.FindAsync(id);
            if (mOrden == null)
            {
                return NotFound();
            }
            if (mOrden.Estado != OrdenEstado.Confirmed)
            {
                _context.mOrden.Remove(mOrden);
                await _context.SaveChangesAsync();
                return Ok("Orden " + mOrden.Id + " eliminada con éxito");

            }
            else
                return BadRequest("Error la orden ya ha sido confirmada");  
        }

        private bool mOrdenExists(int id)
        {
            return _context.mOrden.Any(e => e.Id == id);
        }
    }
}
