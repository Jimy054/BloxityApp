using System.ComponentModel.DataAnnotations;

namespace BloxityApp.Models
{
    public class ProductoModel
    {
        public long ProductoId { get; set; }
        public long? ProveedorId { get; set; }
        [StringLength(20)]
        [Required(ErrorMessage = "El campo Codigo es obligatorio")]
        public string Codigo { get; set; } = null!;
        [StringLength(150)]
        [Required(ErrorMessage = "El campo Descripción es obligatorio")]
        public string Descripcion { get; set; } = null!;
        [StringLength(3)]
        [Required(ErrorMessage = "El campo Unidad es obligatorio")]
        public string Unidad { get; set; } = null!;

        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "El valor debe de tener 2 decimales")]
        [Required(ErrorMessage = "El campo Costo es obligatorio")]
        public decimal Costo { get; set; }
        public string? Estado { get; set; }

        [Display(Name = "Fecha De Creacion")]
        public DateTime? FechaDeCreacion { get; set; }

        [Display(Name = "Fecha De Modificacion")]
        public DateTime? FechaDeModificacion { get; set; }
        public DateTime? FechaDeEliminacion { get; set; }


        public virtual ProveedorModel? Proveedor { get; set; }
    }
}
