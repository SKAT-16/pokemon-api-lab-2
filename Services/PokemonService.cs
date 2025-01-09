using System.Collections.Generic;
using MongoDB.Driver;
using pokemon_api.Models;

namespace PokemonApi.Services
{
  public interface IPokemonService
  {
    Task<IEnumerable<Pokemon>> GetAll();
    Task<Pokemon> GetById(string id);
    Task AddPokemon(Pokemon pokemon);
    Task UpdatePokemon(string id, Pokemon updatedPokemon);
    Task DeletePokemon(string id);
  }

  public class PokemonService : IPokemonService
  {
    private readonly IMongoCollection<Pokemon> _pokemonCollection;

    public PokemonService(IConfiguration config)
    {
      var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
      var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);

      _pokemonCollection = database.GetCollection<Pokemon>(config["MongoDbSettings:CollectionName"]);
    }

    public async Task<IEnumerable<Pokemon>> GetAll()
    {
      return await _pokemonCollection.Find<Pokemon>(_ => true).ToListAsync();
    }

    public async Task<Pokemon> GetById(string id)
    {
      return await _pokemonCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddPokemon(Pokemon pokemon)
    {
      await _pokemonCollection.InsertOneAsync(pokemon);
    }

    public async Task UpdatePokemon(string id, Pokemon updatedPokemon)
    {
      var filter = Builders<Pokemon>.Filter.Eq(p => p.Id, id);
      var update = Builders<Pokemon>.Update
        .Set(p => p.Name, updatedPokemon.Name)
        .Set(p => p.Type, updatedPokemon.Type)
        .Set(p => p.Ability, updatedPokemon.Ability)
        .Set(p => p.Level, updatedPokemon.Level);


      await _pokemonCollection.UpdateOneAsync(filter, update);
    }

    public async Task DeletePokemon(string id)
    {
      var pokemon = _pokemonCollection.Find(p => p.Id == id).FirstOrDefault();
      if (pokemon == null)
        throw new KeyNotFoundException($"No Pokémon found with ID {id}.");

      await _pokemonCollection.DeleteOneAsync(p => p.Id == id);
    }
  }
}
