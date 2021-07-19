using Microsoft.EntityFrameworkCore;

namespace AgendaApi.Models
{
    public class AgendaContext : DbContext
    {
        public AgendaContext(DbContextOptions<AgendaContext> options)
            : base(options)
        {
        }

        public DbSet<AgendaItem> agendaItems { get; set; }
    }
}