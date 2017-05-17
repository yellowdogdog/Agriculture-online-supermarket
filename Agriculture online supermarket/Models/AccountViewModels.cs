using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agriculture_online_supermarket.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 0:正常
        /// -1:用户名错误
        /// -2:密码错误
        /// </summary>
        public int ErrorState
        {
            get;set;
        }
        
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "昵称")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "地址")]

        public string Address { get; set; }
        [Display(Name = "电话")]
        [StringLength(11, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 8)]
        public string Phone { get; set; }

    }
    public class RegisterSellerModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string SellerId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
        [Display(Name ="昵称")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "地址")]
        
        public string Address { get; set; }
        [Display(Name = "电话")]
        [StringLength(11, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 8)]
        public string Phone { get; set; }

    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }
    
    public class BalanceModel
    {
        public double Balance { get; set; }
    }

    public class UserinfoModel
    {   //信息修改界面
        public string ID { get; set; }//用户ID
        public string name { get; set; }//昵称
        public string passward { get; set; }//密码
        public string adress { get; set; }//地址
        public string phoneNumber { get; set; }//电话号码
        public double  balance { get; set; }//账户余额
        public UserinfoModel(string ID,string name,string passward,string adress, string phoneNumber, double balance)
        {
            this.ID = ID;
            this.name = name;
            this.passward = passward;
            this.adress = adress;
            this.phoneNumber = phoneNumber;
            this.balance = balance;
        }
    }
    public class AdminIndexViewModel
    {
        public string userName
        {
            get;set;
        }
        public string userId
        {
            get;set;
        }
        public int type
        {
            get;set;
        }
    }
}
