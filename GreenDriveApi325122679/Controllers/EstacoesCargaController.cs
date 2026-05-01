using GreenDriveApi.Data;
using GreenDriveApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstacoesCargaController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstacoesCargaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstacaoCarga>>> Get()
    {
        return await _context.EstacoesCarga.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstacaoCarga>> GetById(int id)
    {
        var estacao = await _context.EstacoesCarga.FindAsync(id);

        if (estacao is null)
        {
            return NotFound("Estacao nao encontrada.");
        }

        return estacao;
    }

    [HttpPost]
    public async Task<ActionResult<EstacaoCarga>> Post(EstacaoCarga estacao)
    {
        _context.EstacoesCarga.Add(estacao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = estacao.Id }, estacao);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, EstacaoCarga estacao)
    {
        var estacaoAtual = await _context.EstacoesCarga.FindAsync(id);

        if (estacaoAtual is null)
        {
            return NotFound("Estacao nao encontrada.");
        }

        estacaoAtual.Localizacao = estacao.Localizacao;
        estacaoAtual.TipoCarga = estacao.TipoCarga;
        estacaoAtual.CargaDisponivelKW = estacao.CargaDisponivelKW;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var estacao = await _context.EstacoesCarga.FindAsync(id);

        if (estacao is null)
        {
            return NotFound("Estacao nao encontrada.");
        }

        _context.EstacoesCarga.Remove(estacao);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
