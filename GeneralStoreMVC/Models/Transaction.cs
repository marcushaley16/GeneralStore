using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreMVC.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}