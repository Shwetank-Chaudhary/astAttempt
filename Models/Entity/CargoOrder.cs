using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astAttempt.Models.Entity
{
    public class CargoOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]

        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
    }
}
