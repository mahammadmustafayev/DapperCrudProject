using Dapper;
using DapperCrud.DTOs;
using DapperCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    

    private readonly IConfiguration _config;
    public SuperHeroController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        IEnumerable<SuperHero> heroes = await SelectAllSuperHeroes(connection);
        //var heroes = await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHero");
        return Ok(heroes);
    }
    [HttpGet("{searchWord}")]
    public async Task<ActionResult<List<SuperHero>>> SearchHeroes(string searchWord)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        var heroes= await connection.QueryAsync<SuperHero>($"SELECT * FROM SuperHero WHERE HeroName LIKE '%{searchWord}%' OR FirstName LIKE '%{searchWord}%' OR LastName LIKE '%{searchWord}%' OR Place LIKE '%{searchWord}%' ");
        return Ok(heroes);
    }
    [HttpGet("{heroId}")]
    public async Task<ActionResult<SuperHero>> GetSuperHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        var hero = await connection.QueryAsync<SuperHero>($"SELECT * FROM SuperHero  WHERE Id = {heroId}");
        //var hero = await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHero  WHERE Id = @Id",new {Id=heroId});
        
        return Ok(hero);

    }
    [HttpPost]
    public async Task<ActionResult<List<SuperHero>>> CreateHero(SuperHeroDto superHero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        await connection.ExecuteAsync("INSERT INTO SuperHero (HeroName, FirstName, LastName, Place) VALUES (@HeroName,@FirstName,@LastName,@Place)",superHero);
        return Ok(await SelectAllSuperHeroes(connection));
    }

    [HttpPut]
    public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero superHero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        await connection.ExecuteAsync("UPDATE SuperHero SET HeroName=@HeroName, FirstName=@FirstName, LastName=@LastName, Place=@Place WHERE Id=@Id", superHero);
        return Ok(await SelectAllSuperHeroes(connection));
    }

    [HttpDelete("{heroId}")]
    public async Task<ActionResult<List<SuperHero>>> DeleteHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        await connection.ExecuteAsync("DELETE FROM SuperHero  WHERE Id=@Id", new {Id=heroId});
        return Ok(await SelectAllSuperHeroes(connection));
    }
    private static async Task<IEnumerable<SuperHero>> SelectAllSuperHeroes(SqlConnection connection)
    {
        return await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHero");
    }
}
