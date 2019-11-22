using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stonk.Models.Persons;
using Stonk.Plugins.Database;
using Stonk.Views.Users;
using Stonk.Models;

namespace Stonk.Controllers
{
    public class UsersController : Controller
    {
        private readonly Database _context;

        public UsersController(Database context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Portfolio(int? id)
        {
            var view = new PortfolioView();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }
            view.user = user;

            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(m => m.userId == user.id);
            if (portfolio == null)
            {
                return NotFound();
            }
            //var sharess = _context.Share.Select(i => new IEnumerable<Share>{ i.portfolioId, portfolio.id });
            List<Share> shares = new List<Share>();
            foreach (var item in _context.Share)
            {
                if (item.portfolioId == portfolio.id)
                    shares.Add(item);
            }
            view.shares = shares;

            List<Stock> stocks = new List<Stock>();
            List<StockValueInTime> values = new List<StockValueInTime>();
            foreach (var item in _context.Stock)
            {
                if (shares.Any(x => x.stockId == item.id && item.id != 0))
                {
                    stocks.Add(item);
                    values.Add(await _context.StockValuesInTime.FirstAsync());//spatne ale fakci
                }

            }
            view.stocks = stocks;
            view.values = values;

            return View(view);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("id,isAdmin,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.isAdmin = false;
                _context.Add(user);
                await _context.SaveChangesAsync();
                Portfolio portfolio = new Portfolio(user.id, 10000);
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Portfolio), user.id);
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,isAdmin,username,password")] User user)
        {
            if (id != user.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
