using System;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace EFCore.PG_765
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = $"Host=localhost;Port=5432;Username={args[0]};Password={args[1]};Database=some_context;";

            using (var ctx = new SomeContext(connection))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                ctx.SomeModel.Add(new SomeModel { Id = 1, Guid = null });
                ctx.SaveChanges();
            }

            Console.WriteLine("Completed successfully");
        }
    }

    public class SomeModel
    {
        public int Id { get; set; }

        public Guid? Guid { get; set; }
    }

    public class SomeContext : DbContext
    {
        readonly string _connection;

        public DbSet<SomeModel> SomeModel { get; set; }

        public SomeContext(string connection) => _connection = connection;

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.UseNpgsql(_connection);

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.Entity<SomeModel>()
                      .HasIndex(x => x.Guid)
                      .IsUnique(false);
    }
}