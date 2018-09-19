using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShopMe.Domains.Common;

namespace ShopMe.Domains
{
    public class OrderDefaultViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = CommonNotification.ERROR_MESSAGE_FIELD_NOT_NULL)]
        [MaxLength(256, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string CustomerName { set; get; }

        public string CustomerBillingAddress { set; get; }

        [Required(ErrorMessage = CommonNotification.ERROR_MESSAGE_FIELD_NOT_NULL)]
        [MaxLength(256, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string CustomerEmail { set; get; }

        [Required(ErrorMessage = CommonNotification.ERROR_MESSAGE_FIELD_NOT_NULL)]
        [MaxLength(256, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_50)]
        public string CustomerMobile { set; get; }

        [MaxLength(256, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string CustomerMessage { set; get; }

        [MaxLength(256, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string PaymentMethod { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public int PaymentStatus { set; get; }

        public int Status { set; get; }

        public string CustomerShippingAddress { get; set; }

        [StringLength(128, ErrorMessage = CommonNotification.ERROR_MESSAGE_MAX_LENGTH_256)]
        public string CustomerId { get; set; }

        public decimal SubTotal { get; set; }
    }


    [Serializable]
    public class OrderShowViewModel : OrderDefaultViewModel
    {
        public virtual IEnumerable<OrderDetailShowViewModel> OrderDetails { get; set; }
    }
}