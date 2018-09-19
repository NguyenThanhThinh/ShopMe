using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMe.Entities.Models
{
    [Table("Slides")]
  public  class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [MaxLength(256)]
        public string Description { set; get; }
        [MaxLength(256)]
        public string Image { set; get; }
        public int? DisplayOrder { set; get; }
        public bool Status { set; get; }

    }
}
