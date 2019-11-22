using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stonk.Models
{
    public class Transaction
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Portfolio")]
        public int portfolioId { get; set; }
        public decimal value { get; set; }
        public bool verified { get; set; }
        public decimal cash { get; set; }
        public decimal assets { get; set; }
    }
}
