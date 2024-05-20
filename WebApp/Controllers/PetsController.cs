using Microsoft.AspNetCore.Mvc;
using WebApp.Data.Repositories;
using WebApp.Filters;

namespace WebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class PetsController : ControllerBase
{
    private readonly IPetsRepository _petsRepository;


    public PetsController(IPetsRepository petsRepository)
    {
        _petsRepository = petsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]PetsFilter filter)
    {
        var pets = _petsRepository.GetPets(filter);

        return Ok(pets);
    }
}