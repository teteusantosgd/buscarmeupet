using MongoDB.Driver;
using WebApp.Models;

namespace WebApp.Data;

public class AppDbContext
{
    private IMongoDatabase _mongoDb;

    public AppDbContext(string dataBase, string connection)
    {
        var settings = MongoClientSettings.FromConnectionString(connection);
        _mongoDb = new MongoClient(settings).GetDatabase(dataBase);
    }

    public IMongoCollection<Pet> Pets
    {
        get
        {
            return _mongoDb.GetCollection<Pet>("Pets");
        }
    }
}
