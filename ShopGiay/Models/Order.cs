namespace ShopGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [StringLength(50)]
        public string OrderId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string ProductId { get; set; }

        public double? TotalPrice { get; set; }

        [StringLength(50)]
        public string PaymentMethodsId { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        public virtual Account Account { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
