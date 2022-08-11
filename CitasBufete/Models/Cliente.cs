using System.ComponentModel.DataAnnotations;

namespace CitasBufete.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Campo requerido")]
        [Display(Name="Nombre completo")]
        public string Nombre_completo { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Medio de pago")]
        public string Medio_pago { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fecha de  nacimiento")]
        [DataType(DataType.Date)]
        public DateTime Fecha_nacimiento { get; set; }

    }
}
