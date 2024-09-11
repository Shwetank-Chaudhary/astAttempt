using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astAttempt.Models.Entity
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
    }
}
