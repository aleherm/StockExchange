using StockExchange.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace StockExchange.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StockExchange.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
