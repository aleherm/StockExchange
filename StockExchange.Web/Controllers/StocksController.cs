using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using StockExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StockExchange.Controllers
{
    // Require authorization for the controller
    [Authorize]
    public class StocksController : Controller
    {
        // Dependency injection
        private ApplicationUserManager UserManager;
        private ApplicationDbContext Context;

        public StocksController(ApplicationUserManager userManager, ApplicationDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }


        // GET: Stocks
        public ActionResult Index()
        {
            // Catch connection errors, redirect to another view if something went wrong
            var stockPrices = new StocksJsonData();
            try
            {
                stockPrices = GetStockPrices();
            }
            catch (WebException e)
            {
                return View("ConnectionError", new ResponseFormat() { Message = e.ToString() });
            }

            var ownedStocks = GetUserOwnedStocks();
            var siteOwnedStocks = GetSiteOwnedStocks();

            var model = new StocksIndexViewModel
            {
                StockPrices = stockPrices.Items,
                PublicationDate = TimeZoneInfo.ConvertTime(stockPrices.PublicationDate, TimeZoneInfo.Local),
                AccountBalance = GetUserAccountBalance(),
                OwnedStocks = ownedStocks,
                StocksTableData = GenerateTableData(stockPrices.Items, ownedStocks, siteOwnedStocks),
                SiteOwnedStocks = siteOwnedStocks
            };

            return View(model);
        }

        // GET: Stocks/Stocks
        // Returns "Stocks" partial view
        public ActionResult Stocks()
        {
            // Catch connection errors, redirect to another view if something went wrong
            var ownedStocks = GetUserOwnedStocks();
            var siteOwnedStocks = GetSiteOwnedStocks();
            var stockPrices = new StocksJsonData();
            try
            {
                stockPrices = GetStockPrices();
            }
            catch (WebException exception)
            {
                return View("ConnectionError", new ResponseFormat() { Message = exception.ToString() });
            }

            UpdateGraphData(stockPrices);

            var model = new StocksIndexViewModel
            {
                StockPrices = stockPrices.Items,
                PublicationDate = TimeZoneInfo.ConvertTime(stockPrices.PublicationDate, TimeZoneInfo.Local),
                AccountBalance = GetUserAccountBalance(),
                OwnedStocks = ownedStocks,
                StocksTableData = GenerateTableData(stockPrices.Items, ownedStocks, siteOwnedStocks),
                SiteOwnedStocks = siteOwnedStocks
            };

            return PartialView("Stocks", model);
        }

        // GET: Stocks/DrawGraph
        // Action for drawing the graph
        public ActionResult DrawGraph()
        {
            return PartialView("DrawGraph");
        }

        // Sell task
        // String company : Alphanumeric code identifying the company whose stocks the user wants to sell.
        // Integer amount : Number of stocks to sell.
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Sell(string company, int amount)
        {
            var stockPrices = GetStockPrices();
            var currentPrice = (decimal)Math.Round(stockPrices.Items.Single(p => p.Code == company).Price, 2);
            var result = await UpdateUserStocks(company, amount, currentPrice, false);
            var ownedStocks = GetUserOwnedStocks();
            var siteOwnedStocks = GetSiteOwnedStocks();

            var model = new StocksIndexViewModel
            {
                StockPrices = stockPrices.Items,
                PublicationDate = TimeZoneInfo.ConvertTime(stockPrices.PublicationDate, TimeZoneInfo.Local),
                Success = result.Success,
                Message = result.Message,
                AccountBalance = GetUserAccountBalance(),
                OwnedStocks = ownedStocks,
                StocksTableData = GenerateTableData(stockPrices.Items, ownedStocks, siteOwnedStocks),
                SiteOwnedStocks = siteOwnedStocks
            };

            return PartialView("Stocks", model);
        }

        // Buy task
        // String company : Alphanumeric code identifying the company whose stocks the user wants to buy.
        // Integer amount : Number of stocks to buy.
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Buy(string company, int amount)
        {
            var stockPrices = GetStockPrices();
            var currentPrice = (decimal)Math.Round(stockPrices.Items.Single(p => p.Code == company).Price, 2);
            var result = await UpdateUserStocks(company, amount, currentPrice, true);
            var ownedStocks = GetUserOwnedStocks();
            var siteOwnedStocks = GetSiteOwnedStocks();

            var model = new StocksIndexViewModel
            {
                StockPrices = stockPrices.Items,
                PublicationDate = TimeZoneInfo.ConvertTime(stockPrices.PublicationDate, TimeZoneInfo.Local),
                Success = result.Success,
                Message = result.Message,
                AccountBalance = GetUserAccountBalance(),
                OwnedStocks = ownedStocks,
                StocksTableData = GenerateTableData(stockPrices.Items, ownedStocks, siteOwnedStocks),
                SiteOwnedStocks = siteOwnedStocks
            };
            return PartialView("Stocks", model);
        }

        // HTTP GET the stock prices from remote server
        [NonAction]
        public StocksJsonData GetStockPrices()
        {
            var wc = new WebClient { Proxy = null };
            var json = "";
            try
            {
                json = wc.DownloadString(StockExResr.Price_Provider_Url);
            }
            catch (WebException e)
            {
                throw e;
            }
            return JsonConvert.DeserializeObject<StocksJsonData>(json);
        }

        // Updates session data with recent readings to be displayed in the graph
        // StockJsonData stockPrices : Object containing price data with the date of publishing
        [NonAction]
        public void UpdateGraphData(StocksJsonData stockPrices)
        {

            var publicationDate = TimeZoneInfo.ConvertTime(stockPrices.PublicationDate, TimeZoneInfo.Local); // Timezone fix


            // Check if the List was created, do so if not.
            if (HttpContext.Session[StockExResr.Graph_Times_Var] == null)
            {
                HttpContext.Session[StockExResr.Graph_Times_Var] = new List<string>();
            }

            // Add the time of the reading
            var times = (List<string>)HttpContext.Session[StockExResr.Graph_Times_Var];
            if (times.Count != 0)
            {
                // Return the function if the last added time is equal to the new one, no point updating.
                if (times[times.Count - 1] == publicationDate.ToString("HH:mm:ss"))
                {
                    return;
                }
            }

            // Limit the readings to 20
            if (times.Count > 20)
            {
                times.RemoveAt(0);
            }
            times.Insert((times.Count), publicationDate.ToString("HH:mm:ss")); // Add new date at the end
            HttpContext.Session[StockExResr.Graph_Times_Var] = times; // Save

            // Loop through companies
            for (var i = 0; i < stockPrices.Items.Count; i++)
            {
                // Check if the List was created, do so if not.
                if (HttpContext.Session[StockExResr.Graph_Prefix + i] == null)
                {
                    HttpContext.Session[StockExResr.Graph_Prefix + i] = new List<decimal>();
                }

                // Add prices for each company
                var prices = (List<decimal>)HttpContext.Session[StockExResr.Graph_Prefix + i];
                if (prices.Count > 20)
                {
                    prices.RemoveAt(0);
                }

                prices.Insert((prices.Count), (decimal)Math.Round((double)stockPrices.Items[i].GetType().GetProperty("Price").GetValue(stockPrices.Items[i]), 2));
                HttpContext.Session[StockExResr.Graph_Prefix + i] = prices;
            };
        }

        // Main method used for updating user information whenever they buy or sell stocks.
        // String company : Alphanumeric code identifying the company whose stocks the user wants to buy/sell. The codes are provided by a remote server.
        // Integer amount : Number of stocks to buy/sell.
        // Decimal price : The current price of the stocks.
        // Bool action : The type of action to perform, true = sell, false = buy.
        [NonAction]
        public async System.Threading.Tasks.Task<ResponseFormat> UpdateUserStocks(string company, int amount, decimal price, bool action)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            var siteStocks = Context.SiteOwnedStocks.Find(StockExResr.Site_Owned_Id).OwnedStocks;

            var currentUserStocks = currentUser.OwnedStocks.First(stock => stock.Name == company).Value;
            var modifiedUserStocks = new int();

            var currentSiteStocks = siteStocks.First(stock => stock.Name == company).Value;
            var modifiedSiteStocks = new int();

            var currentBalance = currentUser.AccountBalance;
            var modifiedAccountBalance = new decimal();

            var response = new ResponseFormat();

            if (action)
            {
                if (currentBalance < (price * amount))
                {
                    response.Message = StockExResr.StocksNotEnoughBalance;
                    return response;
                }
                else if (amount > currentSiteStocks)
                {
                    response.Message = String.Format(StockExResr.StocksNotEnoughToSellSite, amount, company, currentSiteStocks);
                    return response;
                }
                else
                {
                    response.Message = String.Format(StockExResr.StocksPurchaseSuccesful, amount, company, (amount * price));
                    modifiedUserStocks = currentUserStocks + amount;
                    modifiedAccountBalance = currentBalance - (price * amount);
                    modifiedSiteStocks = currentSiteStocks - amount;
                }
            }
            else
            {
                if ((currentUserStocks - amount) < 0)
                {
                    response.Message = String.Format(StockExResr.StocksNotEnoughToSellUser, amount, company);
                    return response;
                }
                else
                {
                    response.Message = String.Format(StockExResr.StocksSaleSuccesful, amount, company, (amount * price));
                    modifiedUserStocks = currentUserStocks - amount;
                    modifiedAccountBalance = currentBalance + (price * amount);
                    modifiedSiteStocks = currentSiteStocks + amount;
                }
            }
            siteStocks.First(stock => stock.Name == company).Value = modifiedSiteStocks;
            currentUser.OwnedStocks.First(stock => stock.Name == company).Value = modifiedUserStocks;
            currentUser.AccountBalance = modifiedAccountBalance;
            await Context.SaveChangesAsync();
            await UserManager.UpdateAsync(currentUser);
            response.Success = true;
            return response;
        }

        // Generate an object containing all the data required to build the stocks buy/sell table
        // @IList<Item> priceList : list of prices and stock data received from the external provider 
        // @IList<OwnedStocks> ownedStocks : list of stocks owned by the current user
        [NonAction]
        public List<StocksTableData> GenerateTableData(IEnumerable<Item> pricesList, IEnumerable<OwnedStock> ownedStocks, IEnumerable<OwnedStock> siteOwnedStocks)
        {
            var tableData = new List<StocksTableData>();
            foreach (var companyStock in pricesList)
            {
                var price = Math.Round(companyStock.Price, 2);
                var numberOfStocks = ownedStocks.First(stock => stock.Name == companyStock.Code).Value;
                tableData.Add(new StocksTableData
                {
                    Name = companyStock.Name,
                    Code = companyStock.Code,
                    NumberOfStocks = numberOfStocks,
                    NumberOfSiteStocks = siteOwnedStocks.First(stock => stock.Name == companyStock.Code).Value,
                    Price = price,
                    Value = numberOfStocks * price,
                    StocksPriceId = companyStock.Code + "-price"
                });
            }
            return tableData;
        }

        // Get current user account balance
        [NonAction]
        public decimal GetUserAccountBalance()
        {
            return UserManager.FindById(User.Identity.GetUserId()).AccountBalance;
        }

        // Get current user owned stocks
        [NonAction]
        public IList<OwnedStock> GetUserOwnedStocks()
        {
            return UserManager.FindById(User.Identity.GetUserId()).OwnedStocks;
        }

        // Get site owned stocks
        [NonAction]
        public IList<OwnedStock> GetSiteOwnedStocks()
        {
            return Context.SiteOwnedStocks.Find(StockExResr.Site_Owned_Id).OwnedStocks;
        }
    }
}