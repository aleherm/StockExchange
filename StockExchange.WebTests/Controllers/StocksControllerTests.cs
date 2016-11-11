using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockExchange.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StockExchange.Controllers.Tests
{
    [TestClass()]
    public class StocksControllerTests
    {
        [TestMethod()]
        public void UpdateUserStocksTest()
        {
        
        // Setup
        var ownedStocksTest = new List<OwnedStock>() {
                new OwnedStock
                    {
                        Name = "FP",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPL",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "PGB",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPC",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPA",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "DL24",
                        Value = 0
                    },
            };
            var siteOwnedStocksTest = new List<OwnedStock>() {
                new OwnedStock
                    {
                        Name = "FP",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPL",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "PGB",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPC",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "FPA",
                        Value = 0
                    },
                    new OwnedStock
                    {
                        Name = "DL24",
                        Value = 0
                    },
            };
            var data = new List<SiteOwnedStocks>() {
                new SiteOwnedStocks() { Id = "6esu31wctl", OwnedStocks = siteOwnedStocksTest }
            }.AsQueryable();

            var dbSetMock = new Mock<IDbSet<SiteOwnedStocks>>();
            dbSetMock.Setup(m => m.Provider).Returns(data.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(data.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(data.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(x => x.SiteOwnedStocks).Returns(dbSetMock.Object);

            var dummyUser = new ApplicationUser() { Id = "test", AccountBalance = 1000, OwnedStocks = ownedStocksTest };
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            userStore.Setup(usrStr => usrStr.FindByIdAsync("test")).ReturnsAsync(dummyUser);
            var userManager = new ApplicationUserManager(userStore.Object);

            var controller = new StocksController(userManager, dbContextMock.Object);
            controller.ControllerContext = new ControllerContext(GetMockedHttpContext(), new RouteData(), controller);

            // Test
            var expected = false;
            var actual = controller.UpdateUserStocks("FP", 50, (decimal)5.50, false);

            // Result
            Assert.AreEqual(expected, actual.Result.Success);
        }

        // Create mock HttpContext
        private HttpContextBase GetMockedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            var urlHelper = new Mock<UrlHelper>();

            var requestContext = new Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(context.Object);
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("test");
            request.Setup(req => req.Url).Returns(new Uri("http://www.google.com"));
            request.Setup(req => req.RequestContext).Returns(requestContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(new RouteData());

            return context.Object;
        }
    }
}