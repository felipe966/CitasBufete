using System.ComponentModel.DataAnnotations;

namespace CitasBufete.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        public int Id_cliente { get; set; }
        [Display(Name = "Nombre del cliente")]
        public string Nombre_cliente { get; set; } = "";
        [Required(ErrorMessage = "Campo requerido")]
        public string Especialidad { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name="Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Hora de la cita")]
        [DataType(DataType.Time)]
        public DateTime Hora { get; set; }


    }
}
