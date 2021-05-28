using Microsoft.EntityFrameworkCore;
using Project.Domain;



namespace Project.Repo
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options): base(options){}

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public object Cliente { get; set; }
    }
}
