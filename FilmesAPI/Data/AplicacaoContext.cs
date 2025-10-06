using Microsoft.EntityFrameworkCore;
using EmpregaAPI.Models;

namespace EmpregaAPI.Data
{
    public class AplicacaoContext : DbContext
    {
        public AplicacaoContext(DbContextOptions<AplicacaoContext> options)
            : base(options)
        {
        }

        public DbSet<Curriculo> Curriculos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } 
    }
}