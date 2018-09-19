using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    public class Feedback
    {
        [Key] public int FeedbackID { get; set; }
        public int ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime? CreatedDate { get; set; }
        [StringLength(128)] public string Title { get; set; }
        [StringLength(1048)] public string Content { get; set; }

        public bool Status { get; set; }

        [ForeignKey("ProductID")] public virtual Product Product { set; get; }

        [ForeignKey("UserID")] public virtual User User { set; get; }
    }
}