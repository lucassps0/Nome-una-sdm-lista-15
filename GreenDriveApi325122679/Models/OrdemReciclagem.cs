namespace GreenDriveApi.Models;

public class OrdemReciclagem
{
    public int Id { get; set; }
    public int BateriaId { get; set; }
    public int EstacaoId { get; set; }
    public string Prioridade { get; set; } = string.Empty;
    public decimal CustoProcessamento { get; set; }
}
