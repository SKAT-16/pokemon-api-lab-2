using Microsoft.AspNetCore.Mvc;
using pokemon_api.Models;
using PokemonApi.Services;

namespace PokemonApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PokemonController : ControllerBase
  {
    
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
      _pokemonService = pokemonService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pokemon>>> GetAll()
    {
      var pokemons = await _pokemonService.GetAll();
      if (!pokemons.Any())
        return NotFound("No Pokémon found.");

      return Ok(pokemons);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pokemon>> GetByID(string id)
    {
      var pokemon = await _pokemonService.GetById(id);
      if (pokemon == null)
        return NotFound($"No Pokémon found with ID {id}.");

      return Ok(pokemon);
    }

    [HttpGet("/type/{type}")]
    public async Task<ActionResult<IEnumerable<Pokemon>>> GetByType(string type){
      var pokemons = await _pokemonService.GetByType(type);
      if (!pokemons.Any())
        return NotFound($"No pokemons of type {type} found.");

      return Ok(pokemons);
    } 

    [HttpPost]
    public async Task<ActionResult<Pokemon>> AddPokemon([FromBody] Pokemon pokemon)
    {
      try
      {
        await _pokemonService.AddPokemon(pokemon);

        return CreatedAtAction(nameof(GetByID), new { id = pokemon.Id }, pokemon);
      }
      catch (ArgumentException ex)
      {
        return Conflict(ex.Message);
      }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePokemon(string id, [FromBody] Pokemon updatedPokemon)
    {
      try
      {
        await _pokemonService.UpdatePokemon(id, updatedPokemon);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePokemon(string id)
    {
      try
      {
        await _pokemonService.DeletePokemon(id);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
    }
  }
}
