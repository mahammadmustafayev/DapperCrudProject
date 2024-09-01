using Dapper;
using DapperCrud.DTOs;
using DapperCrud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Interfaces;

public class SuperHeroService : ISuperHeroService
{
    private readonly IConfiguration _config;

    public SuperHeroService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<int> CreateHero(SuperHeroDto superHero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        var newHero= await connection.ExecuteAsync("INSERT INTO SuperHero (HeroName, FirstName, LastName, Place) VALUES (@HeroName,@FirstName,@LastName,@Place)", superHero);
        return newHero;
    }

    public async Task<int> DeleteHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        var hero= await connection.ExecuteAsync("DELETE FROM SuperHero  WHERE Id=@Id", new { Id = heroId });
        return hero;
    }

    public async Task<IEnumerable<SuperHero>> GetAllSuperHeroes()
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        IEnumerable<SuperHero> heroes = await SelectAllSuperHeroes(connection);
        return heroes;
    }

    public async Task<SuperHero> GetSuperHero(int heroId)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        return await connection.QueryFirstOrDefaultAsync<SuperHero>($"SELECT * FROM SuperHero  WHERE Id = {heroId}");
       
        //using (var connection = new SqlConnection(_config.GetConnectionString("Database")))
        //{
        //    return await connection.QueryFirstOrDefaultAsync<SuperHero>("SELECT * FROM Products WHERE Id = @Id", new { Id = heroId });
        //}
    }

    public async Task<IEnumerable<SuperHero>> SearchHeroes(string searchWord)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        return await connection.QueryAsync<SuperHero>($"SELECT * FROM SuperHero WHERE HeroName LIKE '%{searchWord}%' OR FirstName LIKE '%{searchWord}%' OR LastName LIKE '%{searchWord}%' OR Place LIKE '%{searchWord}%' ");
    }

    public async Task<int> UpdateHero(SuperHero superHero)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("Database"));
        var updateHero = await connection.ExecuteAsync("UPDATE SuperHero SET HeroName=@HeroName, FirstName=@FirstName, LastName=@LastName, Place=@Place WHERE Id=@Id", superHero);
        return updateHero;
    }

    private static async Task<IEnumerable<SuperHero>> SelectAllSuperHeroes(SqlConnection connection)
    {
        return await connection.QueryAsync<SuperHero>("SELECT * FROM SuperHero");
    }

    
}
