using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Mvc.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "MustEmailAddress")]
        [Display(Name = "LoginUserName")]
        public string UserName { get; set; }

        [Display(Name = "LoginPassword")]
        [StringLength(8,ErrorMessage = "PwdMust8length")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}