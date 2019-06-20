using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntosData.Context.Interface
{
    public interface IDbContext : IDisposable
    {
        DbContext Instance { get; }
    }
}
