using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stonk.Models;
using Stonk.Models.Persons;
using Stonk.Plugins.Database;
using Stonks.Plugins.Generator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Stonks.Controllers
{
    //[Route("[controller]/[action]")]
    public class GeneratorController : Controller
    {
        private readonly Database _context;




        private ulong Ticks { get; set; } = 0;

        List<Stock> LastIteration { get; set; } = new List<Stock>() { new Stock() { id = 10, firstValue = 10, description = "asas" } };  
        decimal TaxRate { get; set; }
        List<StockDependency> Dependencies { get; set; }

        private int IterationsPerTicks { get; set; } = 1;
        //List<Stock>  { get; set; }

        ConcurrentDictionary<int, int> transactions = new ConcurrentDictionary<int, int>();
        private readonly object _lock = new object();

        public IActionResult Index()
        {
            return new OkResult();
        }
        public GeneratorController(Database context)
        {
            _context = context;
            
            Dependencies = new List<StockDependency>();  //TODO add dependencies
            timer.Elapsed += (a, e) => OneTick();
            //timer.Start();
            timer.Enabled = false;
        }

        Timer timer { get; } = new Timer(1);

        async Task OneTick()
        {
            Generate();
            foreach (var item in LastIteration)
            {
               // _context.Update(item);
            }
            //await _context.SaveChangesAsync();
            Ticks++;
        }


        // Get: Generator/CurrentGameState
        [HttpGet]
        public async Task<IActionResult> CurrentGameState()
        {
            var tmp = LastIteration; //cache the collection
            return View(tmp);

        }

        // Get: Generator/CurrentTime
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CurrentTime()
        {
            var tmp = LastIteration; //cache the collection
            return Json(Ticks);

        }

        [HttpGet()]
        public async Task<IActionResult> GetStockWithTime()
        {
            Generate();
            dynamic obj = LastIteration.First().firstValue;
            return Json(obj);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockWithTime(int id)
        {
             Generate();                 //cache the collection
            return Json(LastIteration.First().firstValue);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CurrentSingleStock(int id)
        {
            var tmp = Generator.RandomlyModify( LastIteration.First());                 //cache the collection
            return View(tmp);
        }

        // POST: Generator/BuyShares
        [HttpPost("{userID}/{stockID}/{amount}/")]
        public async Task<IActionResult> BuyShares(int userID, int stockID, int amount)
        {
            var user = _context.Users.Find(userID);
            var stock = _context.Stock.Find(stockID);

            if (amount > 0 && user.userPortfolio.cash >= stock.firstValue * amount)
            {
                var portfolio = user.userPortfolio;
                user.userPortfolio.cash -= stock.firstValue * amount;

                if (user.userPortfolio.listOfShares.Any(x => x.id == stockID))
                    user.userPortfolio.listOfShares.Find(x => x.id == stockID).amount += amount;
                else
                    user.userPortfolio.listOfShares.Add(new Share()
                    {
                        stockId = stockID,
                        amount = amount,
                        portfolioId = user.userPortfolio.id
                    });

                await _context.SaveChangesAsync();
                transactions.AddOrUpdate(stockID, amount, (_, oldVal) => oldVal + amount);

                return Ok();
            }
            return BadRequest();

        }
        // POST: Generator/BuyShares
        [HttpPost("{userID}/{stockID}/{amount}/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellShares(int userID, int stockID, int amount)
        {
            var user = _context.Users.Find(userID);
            var stock = _context.Stock.Find(stockID);

            if (amount > 0 && user.userPortfolio.listOfShares.Find(x => x.id == stockID).amount >= amount)
            {
                var portfolio = user.userPortfolio;
                user.userPortfolio.cash += stock.firstValue * amount * (1 - TaxRate);

                if (user.userPortfolio.listOfShares.Any(x => x.id == stockID))
                    user.userPortfolio.listOfShares.Find(x => x.id == stockID).amount += amount;
                else
                    user.userPortfolio.listOfShares.Add(new Share()
                    {
                        stockId = stockID,
                        amount = amount,
                        portfolioId = user.userPortfolio.id
                    });

                await _context.SaveChangesAsync();
                transactions.AddOrUpdate(stockID, -amount, (_, oldVal) => oldVal - amount);

                return Ok();
            }
            return BadRequest();

        }

        void Generate()
        {
            var tmp = LastIteration.First();

            LastIteration = new List<Stock>(){Generator.RandomlyModify(tmp)};
        }


        StockValueInTime StockToTimeStock(Stock item)
        {
            return new StockValueInTime()
            {
                stockId = item.id,
                timestamp = Ticks,
                value = item.firstValue
            };
        }

        // both 'last' and 'dependencies' are expected to be (ascending)ordered by id 
        Dictionary<int, double> PropagateDependencies(List<Stock> last, List<StockDependency> dependencies)
        {
            int i = 0;
            var curr = last[i];
            var dict = new Dictionary<int, double>();
            foreach (var dependency in dependencies)
            {
                while (curr.id != dependency.SourceID)
                {
                    if (i != last.Count - 1)
                    {
                        var tmp = curr;
                        curr = last[++i];
                        if (tmp.id > curr.id)
                            throw new Exception("stock-list is not ordered!");
                    }
                    else break;
                }
                if (dict.TryGetValue(dependency.TargetID, out var value))
                {
                    dict[dependency.TargetID] = value + dependency.multiplier * curr.growTrend;
                }
                else
                {
                    dict[dependency.TargetID] = dependency.multiplier * curr.growTrend;
                }
            }
            return dict;
        }

    }
}
