using System;

namespace ShopMe.Domains
{
  public  class FeedbackViewModel
    {
        public int FeedbackID { get; set; }
        public int ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime? CreatedDate { get; set; }
   
        public string Title { get; set; }

        public string Content { get; set; }

        public bool Status { get; set; }

       
        public virtual ProductViewModel Product { set; get; }

    
        public virtual UserViewModel User { set; get; }
    }
}
