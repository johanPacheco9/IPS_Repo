namespace Domain.Enums;
using System.ComponentModel.DataAnnotations;

public enum EstadoPaciente
{
    [Display(Name = "Inactivo")]
    Inactivo = 0,

    [Display(Name = "Activo")]
    Activo = 10,
    
    [Display(Name = "En mora")]
    EnMora = 15,

    [Display(Name = "Retirado")]
    Retirado = 20,

    [Display(Name = "Fallecido")]
    Fallecido = 30
}