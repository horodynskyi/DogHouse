using DogHouse.DAL;
using FluentValidation;

namespace DogHouse.BLL;

public class DogValidator:AbstractValidator<Dog>
{
    private readonly DataContext _context;
    public DogValidator(DataContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotNull().WithMessage("Name must be not null").MustAsync(IsExist)
            .WithMessage("Dog with the same name already exists in DB");
        RuleFor(x => x.Weight).NotNull().WithMessage("Weight must be not null").GreaterThan(0).WithMessage("Weight must be greater than 0");
        RuleFor(x => x.Color).NotNull().WithMessage("Color must be not null").When(x => string.IsNullOrEmpty(x.Color));
        RuleFor(x => x.TailLength).NotNull().WithMessage("TailLength must be not null").GreaterThan(0).WithMessage("TailLength must be greater than 0");
    }
    
    public async Task<bool> IsExist<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity:class
    {
        if (await _context.Set<Dog>().FindAsync(entity) == null)
            return true;
        else return false;
    }
}