using GreenDriveApi.Data;
using GreenDriveApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrosTelemetriaController : ControllerBase
{
    private readonly AppDbContext _context;

    public RegistrosTelemetriaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegistroTelemetria>>> Get()
    {
        return await _context.RegistrosTelemetria.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegistroTelemetria>> GetById(int id)
    {
        var registro = await _context.RegistrosTelemetria.FindAsync(id);

        if (registro is null)
        {
            return NotFound("Registro de telemetria nao encontrado.");
        }

        return registro;
    }

    [HttpPost]
    public async Task<ActionResult<RegistroTelemetria>> Post(RegistroTelemetria registro)
    {
        var bateria = await _context.Baterias.FindAsync(registro.BateriaId);

        if (bateria is null)
        {
            return BadRequest("Bateria informada nao existe.");
        }

        if (registro.Temperatura > 85)
        {
            Console.WriteLine($"ALERTA DE SEGURANÇA: Risco térmico detectado na bateria {bateria.NumeroSerie}! Registro bloqueado para investigação.");
            return BadRequest("Temperatura superior a 85°C. Registro bloqueado para investigação.");
        }

        if (registro.DataLeitura == default)
        {
            registro.DataLeitura = DateTime.Now;
        }

        _context.RegistrosTelemetria.Add(registro);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = registro.Id }, registro);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, RegistroTelemetria registro)
    {
        var registroAtual = await _context.RegistrosTelemetria.FindAsync(id);

        if (registroAtual is null)
        {
            return NotFound("Registro de telemetria nao encontrado.");
        }

        if (!await _context.Baterias.AnyAsync(b => b.Id == registro.BateriaId))
        {
            return BadRequest("Bateria informada nao existe.");
        }

        registroAtual.BateriaId = registro.BateriaId;
        registroAtual.Temperatura = registro.Temperatura;
        registroAtual.Voltagem = registro.Voltagem;
        registroAtual.DataLeitura = registro.DataLeitura;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var registro = await _context.RegistrosTelemetria.FindAsync(id);

        if (registro is null)
        {
            return NotFound("Registro de telemetria nao encontrado.");
        }

        _context.RegistrosTelemetria.Remove(registro);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
