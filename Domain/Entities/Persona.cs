namespace PersonasCRUD.Domain.Entities;

public class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

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