using Dapper;
using DapperCrud.DTOs;
using DapperCrud.Interfaces;
using DapperCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SuperHeroController : ControllerBase
{
    private readonly ISuperHeroService _heroService;

    public SuperHeroController(ISuperHeroService heroService)
    {
        _heroService = heroService;
    }

    [HttpGet]   
    public async Task<IActionResult> GetAllSuperHeroes()
    {
        var heroes= await _heroService.GetAllSuperHeroes();
        if (heroes == null) return NotFound();
        return Ok(heroes);
    }

    [HttpGet("{heroId}")]
    public async Task<IActionResult> GetSuperHero(int heroId)
    {
        var superHero= await _heroService.GetSuperHero(heroId);
        if (superHero == null) return NotFound();
        return Ok(superHero);
    }

    [HttpGet("{searchWord}")]
    public async Task<IActionResult> SearchHeroes(string searchWord)
    {
        var searchHeroes= await _heroService.SearchHeroes(searchWord);
        if (searchHeroes == null) return NotFound();
        return Ok(searchHeroes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHero(SuperHeroDto superHero)
    {
        var newHero= await _heroService.CreateHero(superHero);
        if(newHero == 0) return NotFound();
        return Ok(await _heroService.GetAllSuperHeroes());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateHero(SuperHero superHero)
    {
        var updateHero= await _heroService.UpdateHero(superHero);
        if (updateHero == 0) return NotFound();
        return Ok(await _heroService.GetAllSuperHeroes());
    }

    [HttpDelete("{heroId}")]
    public async Task<IActionResult> DeleteHero(int heroId)
    {
        var deleteHero= await _heroService.DeleteHero(heroId);
        if (deleteHero == 0) return NotFound();
        return Ok(await _heroService.GetAllSuperHeroes());
    }
 
}
