using GreenDriveApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenDriveApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Bateria> Baterias => Set<Bateria>();
    public DbSet<EstacaoCarga> EstacoesCarga => Set<EstacaoCarga>();
    public DbSet<RegistroTelemetria> RegistrosTelemetria => Set<RegistroTelemetria>();
    public DbSet<OrdemReciclagem> OrdensReciclagem => Set<OrdemReciclagem>();
}
