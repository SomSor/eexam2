using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models.AccountViewModels
{
    public class LoginViewModel
    {
        public string Email
        {
            get
            {
                return string.Format("{0}.{1}@{2}", CenterId, Username, "iddriver.com");
            }
        }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Center ID")]
        public string CenterId { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
