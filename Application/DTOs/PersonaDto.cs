using System.ComponentModel.DataAnnotations;

namespace PersonasCRUD.Application.DTOs
{
    public class PersonaDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de persona es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "TipoPersona debe ser un valor válido.")]
        public int TipoPersona { get; set; } // Se mapea al enum `TipoPersona`

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
    }
}

