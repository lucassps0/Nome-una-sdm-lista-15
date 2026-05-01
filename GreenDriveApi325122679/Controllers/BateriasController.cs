using GreenDriveApi.Data;
using GreenDriveApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BateriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public BateriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bateria>>> Get()
    {
        return await _context.Baterias.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Bateria>> GetById(int id)
    {
        var bateria = await _context.Baterias.FindAsync(id);

        if (bateria is null)
        {
            return NotFound("Bateria nao encontrada.");
        }

        return bateria;
    }

    [HttpPost]
    public async Task<ActionResult<Bateria>> Post(Bateria bateria)
    {
        _context.Baterias.Add(bateria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = bateria.Id }, bateria);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Bateria bateria)
    {
        var bateriaAtual = await _context.Baterias.FindAsync(id);

        if (bateriaAtual is null)
        {
            return NotFound("Bateria nao encontrada.");
        }

        bateriaAtual.NumeroSerie = bateria.NumeroSerie;
        bateriaAtual.CapacidadeKWh = bateria.CapacidadeKWh;
        bateriaAtual.SaudeBateria = bateria.SaudeBateria;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id:int}/saude")]
    public async Task<IActionResult> AtualizarSaude(int id, Bateria dados)
    {
        var bateria = await _context.Baterias.FindAsync(id);

        if (bateria is null)
        {
            return NotFound("Bateria nao encontrada.");
        }

        if (bateria.SaudeBateria <= 10 && dados.SaudeBateria > bateria.SaudeBateria)
        {
            return Conflict("Bateria inativa nao pode ter a saude aumentada.");
        }

        bateria.SaudeBateria = dados.SaudeBateria;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var bateria = await _context.Baterias.FindAsync(id);

        if (bateria is null)
        {
            return NotFound("Bateria nao encontrada.");
        }

        _context.Baterias.Remove(bateria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
