﻿using Hackaton.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Api.Data
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

            modelBuilder.Entity<Consulta>()
                .HasOne(a => a.Medico);

            modelBuilder.Entity<Consulta>()
                .HasOne(a => a.Paciente);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextClass).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Cascade;

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Consulta> Agenda { get; set; }
        public virtual DbSet<Medico> Medico { get; set; }
        public virtual DbSet<Paciente> Paciente { get; set; }
        public virtual DbSet<Login> Login { get; set; }
    }
}
