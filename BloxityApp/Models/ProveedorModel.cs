using System.ComponentModel.DataAnnotations;

namespace BloxityApp.Models
{
    public class ProveedorModel
    {
        public long ProveedorId { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "El campo codigo es obligatorio")]
        public string Codigo { get; set; } = null!;

        [Display(Name = "Razon Social")]
        [StringLength(150)]
        [Required(ErrorMessage = "El campo Razon Social es obligatorio")]
        public string RazonSocial { get; set; } = null!;

        [Display(Name = "RFC")]
        [StringLength(13)]
        [RegularExpression(@"[A-ZÑ&]{3,4}\d{6}[A-V1-9][A-Z1-9][0-9A]", ErrorMessage ="El Formato no coincide")]
        [Required(ErrorMessage = "El campo RFC es obligatorio")]
        public string Rfc { get; set; } = null!;

        public string? Estado { get; set; }

        [Display(Name = "Fecha De Creacion")]
        public DateTime? FechaDeCreacion { get; set; }

        [Display(Name = "Fecha De Modificacion")]
        public DateTime? FechaDeModificacion { get; set; }
        public DateTime? FechaDeEliminacion { get; set; }

        
        public virtual ICollection<ProductoModel>? Productos { get; set; }
    }
}
