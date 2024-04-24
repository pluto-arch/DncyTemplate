
#if Tenant
using Dotnetydd.MultiTenancy;
using Dotnetydd.MultiTenancy.Model;
#endif
using DncyTemplate.Mvc.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DncyTemplate.Constants;
using Dotnetydd.Permission.PermissionGrant;

namespace DncyTemplate.Mvc.Controllers
{
    [AutoResolveDependency]
    public partial class AccountController : Controller
    {
        [AutoInject]
        private readonly ILogger<AccountController> _logger;

        [AutoInject]
        private readonly IPermissionGrantStore _permissionGrantStore;

#if Tenant
        [AutoInject]
        private readonly ICurrentTenant _currentTenant;
#endif

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["returnUrl"] = returnUrl;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["returnUrl"] = returnUrl;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login"); ;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = InMemoryAccount.GetUsers().FirstOrDefault(x => x.Account == model.UsernameOrEmailAddress);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.UsernameOrEmailAddress), "用户不存在");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
#if Tenant
                new Claim(AppConstant.TENANT_KEY, user?.Tenant??"default")
#endif
            };
            if (user.Roles.Any())
            {
                foreach (var item in user.Roles)
                {
                    claims.Add(new(ClaimTypes.Role, item.ToString()));
                }
            }

#if Tenant
            // init permission already granted
            using (_currentTenant.Change(new TenantInfo(user.Tenant)))
            {
                List<IPermissionGrant> grantList = [];
                foreach (var ur in user.Roles)
                {
                    var permiss = await _permissionGrantStore.GetListAsync("Role", ur.ToString());
                    grantList.AddRange(permiss);
                }
                grantList.AddRange(await _permissionGrantStore.GetListAsync("User", user.Id));
                claims.Add(new Claim(UserClaimConstantValue.CLAIM_PERMISSION, string.Join("|", grantList.Select(x => x.Name).Distinct())));
            }
#endif
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            _logger.LogInformation("User {Name} logged in at {Time}.", user.Name, DateTime.UtcNow);


            if (Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

    }
}
