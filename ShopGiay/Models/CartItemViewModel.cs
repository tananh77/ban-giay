using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopGiay.Models
{
    public class CartItemViewModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string Image { get; set; }
        public int? Available { get; set; }
    }

}