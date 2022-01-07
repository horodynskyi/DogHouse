using DogHouse.BLL;
using DogHouse.BLL.Params;
using DogHouse.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DogHouse.WEB.Controllers;

public class DogController:ControllerBase
{
    private readonly IDogService _service;

    public DogController(IDogService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("ping")]
    public async Task<IActionResult> Ping()
    {
        return Ok("Dogs house service. Version 1.0.1");
    }
    [HttpGet]
    [Route("dogs")]
    public async Task<IActionResult> Get([FromQuery] DogParams parameters)
    {
        var dogs = await _service.Get(parameters);
        return Ok(dogs);
    }
    [HttpPost]
    [Route("dog")]
    public async Task<IActionResult> Create([FromBody] Dog dog)
    {
        if (dog == null) return BadRequest("Invalid JSON");
        var valResult = await _service.Validation(dog);
        if (valResult.IsValid)
        {
            await _service.Create(dog);
            return Ok();
        }
        return BadRequest(valResult.Errors.Select(x => new { Error = x.ErrorMessage, Code = x.ErrorCode }).ToList());
    }
}