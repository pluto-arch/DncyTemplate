using System.Text.RegularExpressions;
using DncyTemplate.Mvc.Infra.Authorization;
using DncyTemplate.Mvc.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Newtonsoft.Json;

namespace DncyTemplate.Mvc.Controllers
{
    [AutoResolveDependency]
    public partial class AccountController : Controller
    {
        [AutoInject]
        private readonly IStringLocalizer<DataAnnotation> _stringLocalizer;


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm]LoginModel model)
        {
            return View("Login", model);
        }


        [HttpPost("account/postdddd")]
        public IActionResult Postdddd([FromForm] LoginModel model)
        { 
            if (!ModelState.IsValid)
            {
                var sdsd = _stringLocalizer[DataAnnotation.MustEmailAddress];
                return Ok(JsonConvert.SerializeObject(ModelState.Values));
            }
            return Ok("123123");
        }
    }
}
