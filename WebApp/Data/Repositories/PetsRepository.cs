using MongoDB.Driver;
using WebApp.Data;
using WebApp.Data.Repositories;
using WebApp.Filters;
using WebApp.Models;

namespace WebApp.Data.Repositories;

public class PetsRepository: IPetsRepository
{
    private readonly AppDbContext _context;

    public PetsRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Pet> GetPets(PetsFilter filter)
    {
        var query = _context.Pets.AsQueryable().Where(x => x.Tipo == filter.Tipo);

        if (!string.IsNullOrEmpty(filter.Cor))
            query = query.Where(x => x.Cor != null && x.Cor == filter.Cor);

        if (!string.IsNullOrEmpty(filter.Pelagem))
            query = query.Where(x => x.Pelagem != null && x.Pelagem.Contains(filter.Pelagem));

        if (!string.IsNullOrEmpty(filter.Olhos))
            query = query.Where(x => x.Olhos != null && x.Olhos.Contains(filter.Olhos));

        if (!string.IsNullOrEmpty(filter.Focinho))
            query = query.Where(x => x.Focinho != null && x.Focinho.Contains(filter.Focinho));

        // if (!string.IsNullOrEmpty(filter.Raca))
        //     query = query.Where(x => x.Raca != null && x.Raca.Contains(filter.Raca));      

        // if (!string.IsNullOrEmpty(filter.Tamanho))
        //     query = query.Where(x => x.Tamanho != null && x.Tamanho.Contains(filter.Tamanho));

        return query.ToList();
    }
}
