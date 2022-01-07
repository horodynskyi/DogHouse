using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogHouse.BLL;
using DogHouse.BLL.Helpers;
using DogHouse.BLL.Params;
using DogHouse.DAL;
using FluentValidation;
using Moq;
using Xunit;

namespace DogHouse.Tests;

public class DogServiseTests
{
    private Mock<IValidator<Dog>> _validatorMock = new();
    private Mock<ISortHelper<Dog>> _sortHelperMock = new();
    private Mock<IDogRepository> _mockDogRepository = new();
    private Mock<DogParams> _mockParams = new();
    private IDogService _service;

    public DogServiseTests()
    {
        _mockDogRepository.Setup(x => x.Create(It.IsAny<Dog>()));
        _mockDogRepository.Setup(x => x.Get()).ReturnsAsync(new List<Dog>());
        _mockDogRepository.Setup(x => x.GetByName(It.IsAny<string>())).ReturnsAsync(new Dog());
        _service = new DogService(_mockDogRepository.Object,_validatorMock.Object,_sortHelperMock.Object);
    }

    [Fact]
    public async Task DogServiceTests_Get_ReturnsIEnumerableDogs()
    {
        var dogs = await _service.Get(_mockParams.Object);
        Assert.NotNull(dogs);
        _mockDogRepository.Verify(x => x.Get(),Times.Once);
    }

    [Fact]
    public async Task DogServiceTests_GetByName_ReturnsSingleDog()
    {
        var mockDog = new Dog {Name = "dada",Color = "dasda",Weight = 5,TailLength = 6};
        await _service.Create(mockDog);
        var dog = await _service.GetByName(mockDog.Name);
        Assert.NotNull(dog);
        _mockDogRepository.Verify(x => x.Create(It.IsAny<Dog>()));
        _mockDogRepository.Verify(x => x.GetByName(It.IsAny<string>()));
    }
    [Fact]
    public void SortHelperTest_AplySort_ReturnsSortedCollection()
    {
        var collection = new List<Dog>();
        collection.AddRange(new []
        {
            new Dog {Name = "a",Color = "red",Weight = 15,TailLength = 1},
            new Dog {Name = "c",Color = "green",Weight = 1,TailLength = 25},
            new Dog {Name = "b",Color = "green",Weight = 5,TailLength = 16},
            new Dog {Name = "d",Color = "blue",Weight = 23,TailLength = 23},
            new Dog {Name = "e",Color = "blue",Weight = 43,TailLength = 15},
        });
        var sortHelper = new SortHelper<Dog>();
        var sortedDogs = sortHelper.ApplySort(collection.AsQueryable(), "Weight").ToList();
        Assert.NotNull(sortedDogs);
        Assert.Equal(sortedDogs[0],collection[1]);
        Assert.NotEqual(sortedDogs[1],collection[0]);
    }
    [Fact]
    public void PageHelperTest_ToPagedList_ReturnsNextPagedCollection()
    {
        var collection = new List<Dog>();
        collection.AddRange(new []
        {
            new Dog {Name = "a",Color = "red",Weight = 15,TailLength = 1},
            new Dog {Name = "c",Color = "green",Weight = 1,TailLength = 25},
            new Dog {Name = "b",Color = "green",Weight = 5,TailLength = 16},
            new Dog {Name = "d",Color = "blue",Weight = 23,TailLength = 23},
            new Dog {Name = "e",Color = "blue",Weight = 43,TailLength = 15},
        });

        var pagedCollection = PageHelper<Dog>.ToPagedList(collection, 2, 2);
        Assert.NotNull(pagedCollection);
        Assert.Equal(pagedCollection,collection.Skip(2).Take(2));
        Assert.NotEqual(pagedCollection,collection);
    }
}