namespace ShopGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InforAccount")]
    public partial class InforAccount
    {
        [StringLength(50)]
        public string InforAccountId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Phone { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public bool? Sex { get; set; }

        [StringLength(1000000000)]
        public string Image { get; set; }

        public virtual Account Account { get; set; }
    }
}
