using DogHouse.BLL.Helpers;
using DogHouse.BLL.Params;
using DogHouse.DAL;
using FluentValidation;
using FluentValidation.Results;

namespace DogHouse.BLL;

public class DogService:IDogService
{
    private readonly IDogRepository _repository;
    private readonly IValidator<Dog> _validator;
    private readonly ISortHelper<Dog> _sortHelper;

    public DogService(IDogRepository repository, IValidator<Dog> validator, ISortHelper<Dog> sortHelper)
    {
        _repository = repository;
        _validator = validator;
        _sortHelper = sortHelper;
    }

    public async Task Create(Dog dog)
    {
        await _repository.Create(dog);
        await _repository.Complete();
    }

    public async Task<IEnumerable<Dog>> Get(DogParams dogParams)
    {
        var res = await _repository.Get();
        var dogs = res.AsQueryable();
        var sortedDogs = _sortHelper.ApplySort(dogs, dogParams.OrderBy);
        return PageHelper<Dog>.ToPagedList(sortedDogs,dogParams.PageNumber,dogParams.PageSize);
    }

    public async Task<Dog> GetByName(string name)
    {
        return await _repository.GetByName(name);
    }
    public async Task<ValidationResult> Validation(Dog dog)
    {
        return await _validator.ValidateAsync(dog);
    }
}

