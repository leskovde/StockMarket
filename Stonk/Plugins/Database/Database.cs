using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Stonk.Models.Persons;
using Stonk.Models;

namespace Stonk.Plugins.Database
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Share> Share { get; set; }

        public DbSet<StockValueInTime> StockValuesInTime { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<Stonk.Models.Persons.Admin> Admin { get; set; }

    }
}