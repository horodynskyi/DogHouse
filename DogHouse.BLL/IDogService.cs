using DogHouse.BLL.Params;
using DogHouse.DAL;
using FluentValidation.Results;

namespace DogHouse.BLL;

public interface IDogService
{
    Task Create(Dog dog);
    Task<IEnumerable<Dog>> Get(DogParams dogParams);
    Task<Dog> GetByName(string name);
    Task<ValidationResult> Validation(Dog dog);
}