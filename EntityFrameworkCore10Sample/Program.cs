using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using var db = new ApplicationDbContext();

var cities = await db.Cities.Where(c => c.Location.Latitude < 0).ToListAsync();

var newCity = new City
{
    Name = "Milan",
    Location = new()
    {
        Latitude = 45.4642,
        Longitude = 9.1900
    }
};

db.Cities.Add(newCity);
await db.SaveChangesAsync();

await db.Cities.Where(c => c.Name == "Taggia").ExecuteUpdateAsync(c =>
{
    c.SetProperty(c => c.Location.Longitude, 7.850600001);
});

var people = await db.People.IgnoreQueryFilters(["SoftDeleteFilter"])
    .Where(p => p.City == "Paperopoli")
    .ToListAsync();

Console.ReadLine();

public class ApplicationDbContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<Person> People { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinimalDB;Integrated Security=true", sqlOptions =>
        {
            sqlOptions.UseCompatibilityLevel(170);
        });

        options.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);
        options.EnableSensitiveDataLogging();

        options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();

            entity.ComplexProperty(e => e.Location, location =>
            {
                location.ToJson();
            });

        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.City).HasMaxLength(50).IsRequired();

            //entity.HasQueryFilter(p => !p.IsDeleted
            //    && p.TenantId == Guid.Parse("11111111-1111-1111-1111-111111111111"));

            entity.HasQueryFilter("SoftDeleteFilter", p => !p.IsDeleted);
            entity.HasQueryFilter("TenantFilter", p => p.TenantId == Guid.Parse("11111111-1111-1111-1111-111111111111"));

            SeedData(entity);

            static void SeedData(EntityTypeBuilder<Person> entity)
            {
                // Migration initial data.
                var tenantId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var tenantId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

                entity.HasData(
                    new() { Id = Guid.NewGuid(), FirstName = "Marco", LastName = "Minerva", City = "Taggia", TenantId = tenantId1, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Mickey", LastName = "Mouse", City = "Topolinia", TenantId = tenantId1, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Donald", LastName = "Duck", City = "Paperopoli", TenantId = tenantId1, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Minnie", LastName = "Mouse", City = "Topolinia", TenantId = tenantId2, IsDeleted = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Goofy", LastName = "Goof", City = "Topolinia", TenantId = tenantId2, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Daisy", LastName = "Duck", City = "Paperopoli", TenantId = tenantId2, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Pluto", LastName = "Dog", City = "Topolinia", TenantId = tenantId1, IsDeleted = true },
                    new() { Id = Guid.NewGuid(), FirstName = "Scrooge", LastName = "McDuck", City = "Paperopoli", TenantId = tenantId1, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Huey", LastName = "Duck", City = "Paperopoli", TenantId = tenantId1, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Dewey", LastName = "Duck", City = "Paperopoli", TenantId = tenantId2, IsDeleted = false },
                    new() { Id = Guid.NewGuid(), FirstName = "Clarabelle", LastName = "Cow", City = "Topolinia", TenantId = tenantId2, IsDeleted = true }
                );
            }
        });
    }
}

public class City
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required Location Location { get; set; }
}

public class Location
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }
}

public class Person
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string City { get; set; }

    public Guid TenantId { get; set; }

    public bool IsDeleted { get; set; }
}