using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astAttempt.Models.Entity
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public double UnitCost { get; set; }
        public string Description { get; set; } = string.Empty;

        //public int CargoId { get; set; }
        //[ForeignKey("CargoId")]

        //public CargoOrder? cargoOrder { get; set; }
    }
}