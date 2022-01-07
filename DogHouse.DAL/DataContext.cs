using Microsoft.EntityFrameworkCore;

namespace DogHouse.DAL;

public class DataContext:DbContext
{
    
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
        //Database.Ens
    }

    public DataContext()
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-S0326N0;Initial Catalog=DogHouseDb;Integrated Security=True");
        }
    }
    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dog>()
            .HasKey(x => x.Name);
        modelBuilder.Entity<Dog>()
            .HasData(new Dog
            {
                Color = "red & amber",
                Name = "Neo",
                TailLength = 22, 
                Weight = 32
            },
                new Dog
            {
                Color = "black & white",
                Name = "Jessy",
                TailLength = 7, 
                Weight = 14
            });
        base.OnModelCreating(modelBuilder);
    }
}