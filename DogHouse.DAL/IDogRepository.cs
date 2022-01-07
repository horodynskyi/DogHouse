namespace DogHouse.DAL;

public interface IDogRepository
{
    Task Create(Dog dog);
    Task<IEnumerable<Dog>> Get();
    Task<Dog> GetByName(string name);
    Task Complete();
}