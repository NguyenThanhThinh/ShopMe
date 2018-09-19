using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("Provinces")]
    public class Provinces
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        public ICollection<Districts> Districts { get; set; }
    }
}
