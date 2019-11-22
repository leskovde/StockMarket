using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stonk.Models
{
    public class Portfolio
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("User")]
        public int userId { get; set; }
        public List<Share> listOfShares { get; set; }
        public List<Transaction> transactions { get; set; }
        public decimal cash { get; set; }

        public Portfolio(int userId, decimal cash)
        {
            this.userId = userId;
            this.cash = cash;
        }
    }
}
