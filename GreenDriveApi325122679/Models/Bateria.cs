namespace GreenDriveApi.Models;

public class Bateria
{
    public int Id { get; set; }
    public string NumeroSerie { get; set; } = string.Empty;
    public double CapacidadeKWh { get; set; }
    public int SaudeBateria { get; set; }
}
