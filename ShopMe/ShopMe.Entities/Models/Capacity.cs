using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    [Table("Capacitys")]
    public class Capacity
    {
        [Key] public int CapacityID { get; set; }
        [StringLength(256)] public string Name { get; set; }
        public IEnumerable<ProductDetail> ProductDetail { get; set; }
    }
}