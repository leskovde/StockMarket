using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stonk.Models
{
    public class Stock
    {
        [Key]
        public int id { get; set; }
        public string company { get; set; }
        public string stockCode { get; set; }
        public string description { get; set; }
        public decimal firstValue { get; set; }
        public List<StockValueInTime> history { get; set; }
        public double growTrend { get; set; }
    }
}
