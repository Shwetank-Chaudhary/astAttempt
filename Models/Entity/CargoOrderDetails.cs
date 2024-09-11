using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astAttempt.Models.Entity
{
    public class CargoOrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShippmentId { get; set; }
        public int CargoOrderId { get; set; }
        [ForeignKey("CargoOrderId")]
        public CargoOrder? CargoOrder { get; set; }

        
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost
        {
            get => UnitCost * Quantity;
        }
        public string Status { get; set; }


    }
}
