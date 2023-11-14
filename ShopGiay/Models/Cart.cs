namespace ShopGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        [StringLength(50)]
        public string CartId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string ProductId { get; set; }

        public int? Quantity { get; set; }

        [StringLength(50)]
        public string Condition { get; set; }

        public virtual Account Account { get; set; }
    }
}
