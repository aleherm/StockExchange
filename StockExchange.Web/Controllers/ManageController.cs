using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StockExchange.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StockExchange.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationUserManager UserManager;
        private ApplicationSignInManager SignInManager;
        private IAuthenticationManager AuthenticationManager;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            AuthenticationManager = authenticationManager;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index()
        {
            // Dependency injection
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(userId);

            var ownedStocks = GetUserOwnedStocksManage();
            var editWalletViewModel = new EditWalletViewModel()
            {
                OwnedStocks = ownedStocks,
                AccountBalance = GetUserAccountBalanceManage(),
                CompanyCount = ownedStocks.Count,
                NumberOfColumns = ownedStocks.Count / 3,
            };

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                EditWalletViewModel = editWalletViewModel
            };
            return View(model);
        }

        //
        // GET: Manage/EditWallet
        public ActionResult EditWallet()
        {
            var ownedStocks = GetUserOwnedStocksManage();
            var model = new EditWalletViewModel() {
                OwnedStocks = ownedStocks,
                AccountBalance = GetUserAccountBalanceManage(),
                CompanyCount = ownedStocks.Count,
                NumberOfColumns = ownedStocks.Count / 3,
            };
            return PartialView(model);
        }

        //
        // POST: Manage/EditWallet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWallet(EditWalletViewModel model)
        {
            var ownedStocks = GetUserOwnedStocksManage();
            model.OwnedStocks = ownedStocks;
            model.CompanyCount = ownedStocks.Count;
            model.NumberOfColumns = ownedStocks.Count / 3;
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            for (var i = 0; i < currentUser.OwnedStocks.Count; i++)
            {
                currentUser.OwnedStocks[i].Value = model.ModifiedOwnedStocks[i].Value;
            }
            currentUser.AccountBalance = model.ModifiedAccountBalance;
            var result = await UserManager.UpdateAsync(currentUser);
            if (result.Succeeded)
            {
                model.Message = StockExResr.ManageWalletChangeSuccess;
                return PartialView(model);
            }
            AddErrors(result);
            return PartialView(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                model.Message = StockExResr.ManagePasswordChangeSuccess;
                return PartialView(model);
            }
            AddErrors(result);
            return PartialView(model);
        }

        // Get current user account balance
        [NonAction]
        public decimal GetUserAccountBalanceManage()
        {
            return UserManager.FindById(User.Identity.GetUserId()).AccountBalance;
        }

        // Get current user owned stocks
        [NonAction]
        public IList<OwnedStock> GetUserOwnedStocksManage()
        {
            return UserManager.FindById(User.Identity.GetUserId()).OwnedStocks;
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Check if the user has set their password
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        #endregion
    }
}