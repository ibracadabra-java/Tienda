using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TiendaProducto.Models;

namespace TiendaProducto.Data
{
    public class TiendaProductoContext : IdentityDbContext<mUsuario>
    {
        public TiendaProductoContext (DbContextOptions<TiendaProductoContext> options)
            : base(options)
        {
        }

        public DbSet<TiendaProducto.Models.mOrden> mOrden { get; set; }

        public DbSet<TiendaProducto.Models.mProducto> mProducto { get; set; }
    }
}
