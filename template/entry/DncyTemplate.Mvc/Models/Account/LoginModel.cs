using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Mvc.Models.Account
{
    public class LoginModel
    {
        [Display(Name = "LoginUserName")]
        [Required(ErrorMessage = "ValueIsRequired")]
        public string UsernameOrEmailAddress { get; set; }

        [Display(Name = "LoginPassword")]
        [StringLength(8,ErrorMessage = "PwdMust8length")]
        [Required(ErrorMessage = "ValueIsRequired")]
        public string Password { get; set; }

        public string RememberMe { get; set; }

        public string ReturnUrlHash { get; set; }
        public string ReturnUrl { get; set; }
    }
}