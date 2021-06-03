using Microsoft.EntityFrameworkCore;

namespace StreamAlerter.DatabaseRepository.Context
{
    public class StreamAlerterContext : DbContext
    {
        public StreamAlerterContext(DbContextOptions<StreamAlerterContext> options)
            : base(options) { }

        public DbSet<Entities.Streamer> Streamers { get; set; }
    }
}
