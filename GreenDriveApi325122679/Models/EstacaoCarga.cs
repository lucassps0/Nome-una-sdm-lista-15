namespace GreenDriveApi.Models;

public class EstacaoCarga
{
    public int Id { get; set; }
    public string Localizacao { get; set; } = string.Empty;
    public string TipoCarga { get; set; } = string.Empty;
    public double CargaDisponivelKW { get; set; }
}
