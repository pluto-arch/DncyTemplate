using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Mvc.Models.Account
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "MustEmailAddress")]
        [Display(Name = "LoginUserName")]
        public string UserName { get; set; }

        [Display(Name = "LoginPassword")]
        [StringLength(8,ErrorMessage = "PwdMust8length")]
        public string Password { get; set; }
    }
}