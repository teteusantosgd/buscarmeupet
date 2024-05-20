using WebApp.Filters;
using WebApp.Models;

namespace WebApp.Data.Repositories;

public interface IPetsRepository
{
    IEnumerable<Pet> GetPets(PetsFilter filter);
}