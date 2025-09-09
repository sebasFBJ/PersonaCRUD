namespace PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Enums;
public class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    
    public string Apellido { get; set; } = string.Empty;
    
    public TipoPersona TipoPersona { get; set; }
    
    public string Telefono { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }

    public int Edad
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}