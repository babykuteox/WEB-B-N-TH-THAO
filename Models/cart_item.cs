using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CNWEB.Models
{
    public class cart_item
    {
        public product product { get; set; }
        public int quantity { get; set; }
    }
}