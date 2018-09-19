using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    [Table("ProductUnits")]
    public class ProductUnit
    {
        [Key] public int ProductUnitID { get; set; }

        [MaxLength(100)] public string Name { get; set; }
        public bool Status { get; set; }

        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)] public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)] public string UpdatedBy { set; get; }
        public IEnumerable<Product> Product { get; set; }
    }
}