using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pokemon_api.Models
{
  public class Pokemon
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }       // Unique identifier for each Pokémon
    public string? Name { get; set; } // Name of the Pokémon
    public string? Type { get; set; } // Type of the Pokémon (e.g., Fire, Water)
    public string? Ability { get; set; } // Pokémon's special ability (e.g., Blaze)
    public int? Level { get; set; }   // Current evolution level of the Pokémon
  }
}