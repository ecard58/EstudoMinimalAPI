using EstudoMinimalAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace EstudoMinimalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
