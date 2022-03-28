using Microsoft.EntityFrameworkCore;

namespace WebAPI_ExceptionHandling_Net6.Models
{
    public partial class LoggingAPIContext : DbContext
    {
        public LoggingAPIContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Log> Logs { get; set; }
    }
}
