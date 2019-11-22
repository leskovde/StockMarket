using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stonk.Models
{
    public class Share
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Portfolio")]
        public int portfolioId { get; set; }
        [ForeignKey("Stock")]
        public int stockId { get; set; }
        public int amount { get; set; }
    }
}
