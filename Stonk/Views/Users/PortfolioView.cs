using Stonk.Models;
using Stonk.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stonk.Views.Users
{
    public class PortfolioView
    {
        public User user { get; set; }
        public List<Share> shares { get; set; }
        public List<Stock> stocks { get; set; }
        public List<StockValueInTime> values { get; set; }

    }
}
