using GreenDriveApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Controllers;

[ApiController]
[Route("api/intelligence")]
public class GridIntelligenceController : ControllerBase
{
    private readonly AppDbContext _context;

    public GridIntelligenceController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("carbon-footprint")]
    public async Task<IActionResult> CarbonFootprint()
    {
        await Task.Delay(3000);

        var resultado = await _context.OrdensReciclagem
            .Join(
                _context.EstacoesCarga,
                ordem => ordem.EstacaoId,
                estacao => estacao.Id,
                (ordem, estacao) => new
                {
                    Cidade = estacao.Localizacao,
                    ordem.CustoProcessamento
                })
            .GroupBy(item => item.Cidade)
            .Select(grupo => new
            {
                Cidade = grupo.Key,
                CustoTotalProcessamento = grupo.Sum(item => item.CustoProcessamento)
            })
            .ToListAsync();

        return Ok(resultado);
    }
}
