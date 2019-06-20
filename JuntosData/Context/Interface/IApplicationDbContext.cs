using JuntosEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosData.Context.Interface
{
    public interface IApplicationDbContext : IDbContext
    {
        DbSet<Usuario> Usuarios { get; set; }

    }
}
