using GreenDriveApi.Data;
using GreenDriveApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdensReciclagemController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdensReciclagemController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdemReciclagem>>> Get()
    {
        return await _context.OrdensReciclagem.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdemReciclagem>> GetById(int id)
    {
        var ordem = await _context.OrdensReciclagem.FindAsync(id);

        if (ordem is null)
        {
            return NotFound("Ordem de reciclagem nao encontrada.");
        }

        return ordem;
    }

    [HttpPost]
    public async Task<ActionResult<OrdemReciclagem>> Post(OrdemReciclagem ordem)
    {
        var bateria = await _context.Baterias.FindAsync(ordem.BateriaId);
        var estacao = await _context.EstacoesCarga.FindAsync(ordem.EstacaoId);

        if (bateria is null)
        {
            return BadRequest("Bateria informada nao existe.");
        }

        if (estacao is null)
        {
            return BadRequest("Estacao informada nao existe.");
        }

        if (bateria.SaudeBateria > 60)
        {
            return BadRequest("Bateria com saúde superior a 60%. Encaminhe para o programa de Reuso Doméstico (Second Life) em vez de reciclagem.");
        }

        if (estacao.TipoCarga.Equals("Ultra-Rapida", StringComparison.OrdinalIgnoreCase))
        {
            ordem.CustoProcessamento += 250;
        }

        _context.OrdensReciclagem.Add(ordem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = ordem.Id }, ordem);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, OrdemReciclagem ordem)
    {
        var ordemAtual = await _context.OrdensReciclagem.FindAsync(id);

        if (ordemAtual is null)
        {
            return NotFound("Ordem de reciclagem nao encontrada.");
        }

        if (!await _context.Baterias.AnyAsync(b => b.Id == ordem.BateriaId))
        {
            return BadRequest("Bateria informada nao existe.");
        }

        if (!await _context.EstacoesCarga.AnyAsync(e => e.Id == ordem.EstacaoId))
        {
            return BadRequest("Estacao informada nao existe.");
        }

        ordemAtual.BateriaId = ordem.BateriaId;
        ordemAtual.EstacaoId = ordem.EstacaoId;
        ordemAtual.Prioridade = ordem.Prioridade;
        ordemAtual.CustoProcessamento = ordem.CustoProcessamento;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ordem = await _context.OrdensReciclagem.FindAsync(id);

        if (ordem is null)
        {
            return NotFound("Ordem de reciclagem nao encontrada.");
        }

        _context.OrdensReciclagem.Remove(ordem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
