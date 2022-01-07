using Microsoft.EntityFrameworkCore;

namespace DogHouse.DAL;

public class DogRepository:IDogRepository
{
    private readonly DataContext _context;
    private readonly DbSet<Dog> _set;

    public DogRepository(DataContext context)
    {
        _context = context;
        _set = _context.Set<Dog>();
    }

    public async Task Create(Dog dog)
    {
        await _set.AddAsync(dog);
    }

    public async Task<IEnumerable<Dog>> Get()
    {
        return await _set.ToListAsync();
    }

    public async Task<Dog> GetByName(string name)
    {
        return await _set.FirstOrDefaultAsync(x => x.Name == name);
    }
    
    public async Task Complete()
    {
        await _context.SaveChangesAsync();
    }
}