using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stonk.Models
{
    public class StockValueInTime
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Stock")]
        public int stockId { get; set; }
        public decimal value { get; set; }
        public ulong timestamp { get; set; }
    }
}
