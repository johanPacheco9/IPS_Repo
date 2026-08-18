namespace Domain.Entities;

public class Eps
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public bool Activo { get; set; }
}