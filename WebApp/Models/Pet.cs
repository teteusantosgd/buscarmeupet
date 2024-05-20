using MongoDB.Bson;

namespace WebApp.Models;

public class Pet
{
    public ObjectId _id { get; set; }

    public string? Raca { get; set; }
    public string? Cor { get; set; }
    public string? Pelagem { get; set; }
    public string? Olhos { get; set; }
    public string? Focinho { get; set; }
    public string? Tamanho { get; set; }
    public string? Idade { get; set; }
    public string? OutrasInformacoes { get; set; }
    public string? Image { get; set; }
    public string? Abrigo { get; set; }
    public string? Tipo { get; set; }
}