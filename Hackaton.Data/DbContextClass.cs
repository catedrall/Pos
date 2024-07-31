using Hackaton.Models;

namespace Hackaton.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DbContextClass()
        { }

        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextClass).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Agenda> Agenda { get; set; }
        public virtual DbSet<Medico> Medico { get; set; }
        public virtual DbSet<Paciente> Paciente { get; set; }
        public virtual DbSet<Login> Login { get; set; }
    }
}
