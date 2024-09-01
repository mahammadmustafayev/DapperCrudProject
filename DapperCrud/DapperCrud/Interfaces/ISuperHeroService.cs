using DapperCrud.DTOs;
using DapperCrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperCrud.Interfaces;

public interface ISuperHeroService
{
    Task<IEnumerable<SuperHero>> GetAllSuperHeroes();
    Task<IEnumerable<SuperHero>> SearchHeroes(string searchWord);
    Task<SuperHero> GetSuperHero(int heroId);
    Task<int> CreateHero(SuperHeroDto superHero);
    Task<int> UpdateHero(SuperHero superHero);
    Task<int> DeleteHero(int heroId);
}
