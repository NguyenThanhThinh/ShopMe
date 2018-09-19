using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMe.Entities.Models
{
    public class PearsonScore
    {
        [Key] [Column(Order = 0)] public string UserID_1 { get; set; }
        [Key] [Column(Order = 1)] public string UserID_2 { get; set; }
        public double? Sim_Pearson_Score { get; set; }
        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}