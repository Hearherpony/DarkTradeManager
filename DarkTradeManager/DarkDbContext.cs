using Microsoft.EntityFrameworkCore;

namespace DarkTradeManager
{
    public class DarkDbContext : DbContext
    {
        // Параметрless-конструктор, создающий опции подключения
        public DarkDbContext() : base(
            new DbContextOptionsBuilder<DarkDbContext>()
                .UseSqlServer("Data Source=EDELWEISS\\SQLEXPRESS;Initial Catalog=Trade;Integrated Security=True")
                .Options)
        { }

        // Конструктор с параметрами
        public DarkDbContext(DbContextOptions<DarkDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=EDELWEISS\\SQLEXPRESS;Initial Catalog=Trade;Integrated Security=True");
            }
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
    }
}
