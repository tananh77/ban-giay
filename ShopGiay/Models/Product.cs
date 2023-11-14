namespace ShopGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [StringLength(50)]
        public string ProductId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string CategoryId { get; set; }

        [Column(TypeName = "text")]
        public string Descripsion { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public virtual Account Account { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
