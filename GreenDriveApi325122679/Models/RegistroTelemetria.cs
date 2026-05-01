namespace GreenDriveApi.Models;

public class RegistroTelemetria
{
    public int Id { get; set; }
    public int BateriaId { get; set; }
    public double Temperatura { get; set; }
    public double Voltagem { get; set; }
    public DateTime DataLeitura { get; set; }
}
