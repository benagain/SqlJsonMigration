using Microsoft.EntityFrameworkCore;

namespace SqlJsonMigration;

public class Context : DbContext
{
    public DbSet<MyDocument> Documents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb;Database=SqlJsonMigration;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyDocument>()
            .OwnsOne(e => e.Json, builder => builder.ToJson());
    }
}

public class MyDocument
{
    public int Id { get; set; }
    public required TheJsonDocument Json { get; set; }
}

public class TheJsonDocument
{
    public required string Property1 { get; init; }
    public required int Property2 { get; set; }
}